terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=2.96.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "sn_rg" {
  name     = "sohatnotebookrg"
  location = "uksouth"
}
