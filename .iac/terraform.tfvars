# REQUIRED CHANGES:
aws_region         = "us-east-1"                    # Your AWS region
environment        = "dev"                           # Keep or change
project_name       = "ritedevops-ec2-machine"                     # ← CHANGE to your project name
key_pair_name      = "rite-devops-terraform"                   # ← CHANGE to your EC2 key pair name
allowed_ssh_cidr   = ["103.216.69.27/32"]            # ← CHANGE to your IP address

# Optional changes:
instance_type      = "t3.micro"                     # Can change to t3.small, etc.
root_volume_size   = 20                             # Can increase if needed
