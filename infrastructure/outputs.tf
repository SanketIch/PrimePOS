output "s3_bucket_name" {
  description = "Name of the S3 bucket created"
  value       = aws_s3_bucket.primepos_bucket.bucket
}

output "ec2_instance_id" {
  description = "ID of the EC2 instance created"
  value       = aws_instance.primepos_ec2.id
}

output "ec2_public_ip" {
  description = "Public IP of the EC2 instance"
  value       = aws_instance.primepos_ec2.public_ip
}
