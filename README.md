# 🚀 DEPI DevOps Graduation Project

![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-CI/CD-2088FF?logo=github-actions&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Containerized-2496ED?logo=docker&logoColor=white)
![AWS](https://img.shields.io/badge/AWS-EC2-FF9900?logo=amazonaws&logoColor=white)
![Terraform](https://img.shields.io/badge/Terraform-IaC-7B42BC?logo=terraform&logoColor=white)
![Ansible](https://img.shields.io/badge/Ansible-Automation-EE0000?logo=ansible&logoColor=white)
![Kubernetes](https://img.shields.io/badge/Kubernetes-K8s-326CE5?logo=kubernetes&logoColor=white)

A complete DevOps implementation for a microservices-based eCommerce application inspired by AdventureWorks.

## 📌 Features

- Microservices Architecture
- Docker & Docker Compose
- CI/CD using GitHub Actions
- Infrastructure as Code using Terraform
- Configuration Management using Ansible
- AWS EC2 Deployment
- Identity & Authentication using IdentityServer
- Monitoring using Prometheus & Grafana
- Kubernetes Ready

---

# 🏗️ System Architecture

```text
User
  ↓
WebApp
  ↓
├── Identity API
├── Catalog API
├── Basket API
└── Ordering API
      ↓
   RabbitMQ
      ↓
 PostgreSQL
```

---

# ⚙️ DevOps Workflow

```text
Developer
   ↓
Git Push
   ↓
GitHub Actions
   ↓
Build Docker Images
   ↓
Push Images to DockerHub
   ↓
Deploy to AWS EC2
   ↓
Docker Compose
   ↓
Running Containers
```

---

# 🧰 Technologies Used

| Category | Tools |
|---|---|
| Cloud | AWS EC2 |
| Containers | Docker, Docker Compose |
| CI/CD | GitHub Actions |
| IaC | Terraform |
| Configuration Management | Ansible |
| Monitoring | Prometheus, Grafana |
| Messaging | RabbitMQ |
| Database | PostgreSQL |
| Authentication | IdentityServer |
| Backend | .NET Microservices |

---

# ☁️ Terraform Infrastructure

Terraform provisions:

- VPC
- Public Subnets
- Internet Gateway
- Route Tables
- Security Groups
- EC2 Instance

```text
terraform/
├── environments/dev
├── modules/vpc
├── modules/ec2
└── modules/security-groups
```

---

# 🐳 Docker Deployment

Services include:

- WebApp
- Catalog API
- Basket API
- Ordering API
- Identity API
- Webhooks API
- PostgreSQL
- RabbitMQ
- Prometheus
- Grafana

## Run Locally

```bash
cd docker

docker-compose up -d
```

---

# 🔄 CI/CD Pipeline

The pipeline automatically:

1. Builds Docker images
2. Pushes images to DockerHub
3. Connects to AWS EC2
4. Pulls latest images
5. Recreates containers
6. Verifies deployment

## CI/CD Flow

```text
Git Push
   ↓
GitHub Actions
   ↓
Docker Build
   ↓
DockerHub Push
   ↓
SSH to EC2
   ↓
Docker Compose Pull
   ↓
Docker Compose Up
   ↓
Application Live 🚀
```

---

# 📊 Monitoring Stack

Monitoring tools:

- Prometheus
- Grafana

Features:

- Container metrics
- Service monitoring
- Resource usage dashboards

---

# 🔐 Authentication

Authentication handled using IdentityServer with OpenID Connect (OIDC).

Features:

- Secure Login
- Protected Cart & Checkout
- Token-based Authentication

---

# 📸 Screenshots

## 🏠 Home Page

![eShop homepage screenshot](img/eshop_homepage.png)

---

# 🧪 Demo Flow

```text
1. git push
2. GitHub Actions starts
3. Docker images build
4. Images pushed to DockerHub
5. EC2 deployment starts
6. Containers recreated
7. Open Website
8. Login
9. Add To Cart
10. Checkout
```

---

# 🚀 Deployment URL

```text
http://100.48.167.201:5100
```

---

# 📂 Project Structure

```text
.
├── ansible
├── docker
├── src
├── terraform
├── tests
└── .github/workflows
```

---

# 📌 Future Improvements

- Full Kubernetes deployment
- Terraform automation inside CI/CD
- Ansible auto provisioning
- HTTPS with Nginx Reverse Proxy
- Domain Integration

---

# 👨‍💻 Author

DEPI DevOps Graduation Project
