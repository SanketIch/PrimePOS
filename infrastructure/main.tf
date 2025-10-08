resource "aws_s3_bucket" "example" {
  bucket = "primepos-terraform-demo-${random_id.rand.hex}"
}

resource "random_id" "rand" {
  byte_length = 4
}
