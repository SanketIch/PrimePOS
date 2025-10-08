# Random suffix for unique S3 bucket name
resource "random_id" "bucket_suffix" {
  byte_length = 4
}

# S3 bucket
resource "aws_s3_bucket" "primepos_bucket" {
  bucket = "primepos-bucket-${random_id.bucket_suffix.hex}"
  acl    = "private"
}

# Security Group for EC2
resource "aws_security_group" "ec2_sg" {
  name        = "primepos-sg"
  description = "Allow HTTP and SSH"
  vpc_id      = data.aws_vpc.default.id

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Get default VPC
data "aws_vpc" "default" {
  default = true
}

# EC2 Instance
resource "aws_instance" "primepos_ec2" {
  ami           = "ami-0c02fb55956c7d316" # Example: Amazon Linux 2 AMI in us-east-1
  instance_type = var.ec2_instance_type
  key_name      = var.ec2_key_name

  vpc_security_group_ids = [aws_security_group.ec2_sg.id]

  tags = {
    Name = "PrimePOS-EC2"
  }
}
