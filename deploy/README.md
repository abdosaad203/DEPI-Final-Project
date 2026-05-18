# eShop deployment (Docker + EKS)

This folder contains container and Kubernetes assets for the [.NET eShop](https://github.com/dotnet/eShop) sample.

## Architecture

| Layer | Components |
|-------|------------|
| **Infrastructure** | PostgreSQL (pgvector), Redis, RabbitMQ |
| **APIs** | identity-api, basket-api (gRPC), catalog-api, ordering-api, webhooks-api |
| **Workers** | order-processor, payment-processor |
| **UI** | webapp, webhooksclient |

## Local Docker Compose

### Prerequisites

- Docker Engine + Compose v2
- .NET 10 SDK (to build images locally)

### Steps

```bash
cd /path/to/eShop

# 1. Environment
cp docker/.env.example docker/.env
# Edit PUBLIC_* URLs if you change host ports.

# 2. Build all images
./docker/build-images.sh

# 3. Start stack
docker compose -f docker/docker-compose.yml --env-file docker/.env up -d

# 4. Wait for health (first start: DB migrations can take 2–3 minutes)
docker compose -f docker/docker-compose.yml ps
```

### URLs (default)

| Service | URL |
|---------|-----|
| **Store (webapp)** | http://localhost:5100 |
| **Identity** | http://localhost:8081 |
| **Webhooks client** | http://localhost:5102 |
| **RabbitMQ management** | http://localhost:15672 (guest/guest) |

### Test login

After the stack is healthy, register or use seeded users from Identity (see Identity `UsersSeed`).

---

## Amazon EKS deployment

### 1. Build and push images to ECR

```bash
export AWS_REGION=us-east-1
export AWS_ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
export ECR_REGISTRY="${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_REGION}.amazonaws.com"

aws ecr create-repository --repository-name eshop/identity-api --region $AWS_REGION || true
# Repeat for basket-api, catalog-api, ordering-api, webhooks-api,
# order-processor, payment-processor, webapp, webhooksclient

aws ecr get-login-password --region $AWS_REGION | \
  docker login --username AWS --password-stdin $ECR_REGISTRY

./docker/build-images.sh latest

for repo in identity-api basket-api catalog-api ordering-api webhooks-api \
  order-processor payment-processor webapp webhooksclient; do
  docker tag eshop/${repo}:latest $ECR_REGISTRY/eshop/${repo}:latest
  docker push $ECR_REGISTRY/eshop/${repo}:latest
done
```

### 2. Configure cluster access

```bash
aws eks update-kubeconfig --name YOUR_CLUSTER --region $AWS_REGION
```

### 3. Install AWS Load Balancer Controller

Required for `ingress.yaml` (ALB). Follow:
https://docs.aws.amazon.com/eks/latest/userguide/aws-load-balancer-controller.html

### 4. Prepare Kubernetes secrets and config

```bash
cp deploy/kubernetes/secret.example.yaml deploy/kubernetes/secret.yaml
# Edit passwords and connection strings.

# Edit deploy/kubernetes/configmap.yaml:
#   PUBLIC_IDENTITY_URL, PUBLIC_WEBAPP_URL, hostnames, RabbitMQ password in eventbus URLs
```

Uncomment and set `images` in `deploy/kubernetes/kustomization.yaml` to your ECR URIs.

### 5. Deploy

```bash
kubectl apply -f deploy/kubernetes/namespace.yaml
kubectl apply -f deploy/kubernetes/secret.yaml
kubectl apply -k deploy/kubernetes/
```

### 6. DNS and TLS

- Point `shop.eshop.example.com`, `identity.eshop.example.com`, `webhooks.eshop.example.com` to the ALB hostname.
- Set `alb.ingress.kubernetes.io/certificate-arn` in `ingress.yaml` for HTTPS.

### Production recommendations

| Component | Suggested AWS service |
|-----------|---------------------|
| PostgreSQL | **Amazon RDS** (enable `pgvector` on catalog DB) |
| Redis | **Amazon ElastiCache** |
| RabbitMQ | **Amazon MQ** |
| Compute | **EKS** with HPA on webapp / APIs |
| Images | **ECR** + CI pipeline (`docker/build-images.sh`) |
| Secrets | **AWS Secrets Manager** + External Secrets Operator |
| Ingress | **ALB** + ACM certificate |

Remove in-cluster `postgres.yaml` when using RDS and update connection strings in secrets.

---

## Kubernetes monitoring (k3s / EKS)

Plain YAML manifests (no Helm, no Kustomize): **Prometheus**, **Grafana**, **node-exporter**, **kube-state-metrics**.

Scrapes cluster/node/pod metrics and cAdvisor container stats.

### Install

```bash
kubectl apply -f deploy/kubernetes/monitoring/
```

### Access Grafana

```bash
kubectl -n monitoring port-forward svc/grafana 3000:80
```

Open http://localhost:3000 — user `admin`, password `admin` (change `monitoring/grafana.yaml` secret before production).

**Via Traefik (k3s):** edit `monitoring/grafana-ingress.yaml` and point `grafana.local` at your node IP.

### Prometheus UI

```bash
kubectl -n monitoring port-forward svc/prometheus 9090:9090
```

### Uninstall

```bash
kubectl delete -f deploy/kubernetes/monitoring/
```

---

## Files

```
docker/
  Dockerfile           # Multi-service build (PROJECT_PATH + ASSEMBLY_NAME args)
  docker-compose.yml   # Full local stack
  build-images.sh      # Build all service images
  postgres/init-databases.sql
deploy/kubernetes/
  kustomization.yaml
  configmap.yaml
  secret.example.yaml
  infrastructure/      # postgres, redis, rabbitmq
  apps/                # Deployments + Services
  ingress.yaml         # ALB ingress for EKS
  monitoring/          # Prometheus + Grafana (plain YAML)
```

## Application changes for containers

- `ENABLE_HEALTH_CHECKS=true` — exposes `/health` and `/alive` for probes
- `ServiceUrls:*` — overrides Aspire `https+http://` URLs in Docker/K8s
- `IssuerUri` — stable IdentityServer issuer for OIDC across hosts
- `DisableHttpsRedirection=true` — HTTP behind reverse proxy / ALB
