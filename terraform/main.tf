resource "azurerm_resource_group" "az_url_shortener" {
  location = var.location
  name     = var.app_name
}

resource "azurerm_static_site" "tiny_blazor_admin" {
  name                = var.app_name
  resource_group_name = azurerm_resource_group.az_url_shortener.name
  location            = azurerm_resource_group.az_url_shortener.location
  sku_tier            = "Standard"
  sku_size            = "Standard"
}

