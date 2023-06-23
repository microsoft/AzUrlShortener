terraform {
  required_version = ">=0.12"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>2.0"
    }
    random = {
      source  = "hashicorp/random"
      version = ">= 3.4.0"
    }
    # tls = {
    #   source  = "hashicorp/tls"
    #   version = "~>4.0"
    # }
  }
  backend "azurerm" {
    resource_group_name  = "tfstate"
    storage_account_name = "tfstate28423"
    container_name       = "tfstate"
    key                  = "AzUrlShortener.tfstate"
  }
}

provider "azurerm" {
  features {}
}
