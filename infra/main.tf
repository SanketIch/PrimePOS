provider "aws" {
  region = var.aws_region
}

resource "aws_db_instance" "sqlserver" {
  identifier              = "primepos-sqlserver"
  engine                  = "sqlserver-ex"
  engine_version          = "15.00"
  instance_class          = "db.t3.micro"
  allocated_storage       = 20
  username                = var.db_username
  password                = var.db_password
  skip_final_snapshot     = true
  publicly_accessible     = true
}
