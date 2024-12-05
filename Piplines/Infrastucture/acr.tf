resource "azurerm_container_registry" "acr" {
  name = "sohatnotebookacr"
  resource_group_name = azurerm_resource_group.sn_rg.name
  location = azurerm_resource_group.sn_rg.location
  sku = "Basic"
  admin_enabled = true
}