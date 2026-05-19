🚀 DEPI DevOps Graduation Project

A complete DevOps implementation for a microservices-based eCommerce application inspired by AdventureWorks.

This project demonstrates:

Microservices Architecture
Docker & Docker Compose
CI/CD Automation using GitHub Actions
Infrastructure as Code using Terraform
Configuration Management using Ansible
AWS EC2 Deployment
Identity & Authentication using IdentityServer
Monitoring using Prometheus & Grafana
Kubernetes manifests (K8s Ready)
🏗️ System Architecture
                    ┌───────────────┐
                    │     User      │
                    └───────┬───────┘
                            │
                            ▼
                  ┌──────────────────┐
                  │      WebApp      │
                  └────────┬─────────┘
                           │
        ┌──────────────────┼──────────────────┐
        ▼                  ▼                  ▼
┌──────────────┐  ┌────────────────┐  ┌────────────────┐
│ Identity API │  │  Catalog API  │  │   Basket API   │
└──────┬───────┘  └────────────────┘  └──────┬─────────┘
       │                                      │
       ▼                                      ▼
┌──────────────┐                    ┌────────────────┐
│ PostgreSQL   │                    │   RabbitMQ     │
└──────────────┘                    └──────┬─────────┘
                                           │
                                           ▼
                                   ┌────────────────┐
                                   │ Ordering API   │
                                   └────────────────┘
⚙️ DevOps Workflow
Developer
   │
   ▼
Git Push
   │
   ▼
GitHub Actions CI/CD
   │
   ├── Build Docker Images
   ├── Push Images to DockerHub
   └── Deploy to AWS EC2
                 │
                 ▼
         Docker Compose
                 │
                 ▼
        Running Containers
🧰 Technologies Used
Category	Tools
Cloud	AWS EC2
Containers	Docker, Docker Compose
CI/CD	GitHub Actions
Infrastructure as Code	Terraform
Configuration Management	Ansible
Monitoring	Prometheus, Grafana
Messaging	RabbitMQ
Database	PostgreSQL
Authentication	IdentityServer
Backend	.NET Microservices
Frontend	ASP.NET WebApp
Orchestration	Kubernetes Manifests
☁️ Terraform Infrastructure

Terraform provisions:

VPC
Public Subnets
Internet Gateway
Route Tables
Security Groups
EC2 Instance
Infrastructure Structure
terraform/
├── environments/dev
├── modules/vpc
├── modules/ec2
├── modules/security-groups
🐳 Docker Deployment

The application is containerized using Docker.

Services include:

WebApp
Catalog API
Basket API
Ordering API
Identity API
Webhooks API
PostgreSQL
RabbitMQ
Prometheus
Grafana
Run Locally
cd docker

docker-compose up -d
🔄 CI/CD Pipeline

The CI/CD pipeline automatically:

Builds Docker images
Pushes images to DockerHub
Connects to AWS EC2 using SSH
Pulls latest images
Recreates containers
Verifies deployment
CI/CD Flow
Git Push
   ↓
GitHub Actions
   ↓
Build Docker Images
   ↓
Push to DockerHub
   ↓
SSH to EC2
   ↓
Docker Compose Pull
   ↓
Docker Compose Up
   ↓
Application Live 🚀
📊 Monitoring Stack

Monitoring is implemented using:

Prometheus
Grafana
Monitoring Features
Container metrics
Service monitoring
Resource usage dashboards
Health monitoring
🔐 Authentication Flow

Authentication is handled using IdentityServer.

Authentication Features
OpenID Connect (OIDC)
Secure login flow
Protected cart & checkout
Token-based authentication
📸 Project Screenshots
🏠 Home Page

🧪 Demo Flow
1. git push
2. GitHub Actions starts
3. Docker images are built
4. Images pushed to DockerHub
5. EC2 deployment starts
6. Containers recreated
7. Open Website
8. Login
9. Browse Products
10. Add To Cart
11. Checkout
🚀 Deployment URL
http://100.48.167.201:5100
📂 Project Structure
.
├── ansible
├── build
├── deploy
├── docker
├── src
├── terraform
├── tests
└── .github/workflows
📌 Future Improvements
Full Kubernetes deployment
Terraform automation inside CI/CD
Ansible auto provisioning in pipeline
HTTPS with Nginx Reverse Proxy
Domain name integration
Auto scaling infrastructure
👨‍💻 Author

DEPI DevOps Graduation Project
