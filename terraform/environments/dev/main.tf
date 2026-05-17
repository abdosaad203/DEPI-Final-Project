module "vpc" {
  source = "../../modules/vpc"

  vpc_name = "depi-vpc"
  vpc_cidr = "10.0.0.0/16"

  public_subnet_1_cidr = "10.0.1.0/24"
  public_subnet_2_cidr = "10.0.2.0/24"

  az_1 = "us-east-1a"
  az_2 = "us-east-1b"
}

module "security_groups" {
  source = "../../modules/security-groups"

  sg_name = "depi-main-sg"
  vpc_id  = module.vpc.vpc_id
}

module "ec2" {
  source = "../../modules/ec2"

  ami_id            = "ami-0fc5d935ebf8bc3bc"
  instance_type     = "t3.micro"

  subnet_id         = module.vpc.public_subnet_1_id
  security_group_id = module.security_groups.security_group_id

  key_name      = "depi-key"
  instance_name = "depi-server"
}