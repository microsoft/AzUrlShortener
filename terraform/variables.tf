variable "location" {
  default     = "centralus"
  description = "Azure Region (eastus, centralus, etc.)"
  type        = string
}

variable "app_name" {
  default     = "AzUrlShortener"
  description = "Application Name"
  type        = string
}

