resource "azurerm_sql_server" "sohat_db_server" {
  name = "sohatdbserver"
  resource_group_name = azurerm_resource_group.sn_rg.name
  location = azurerm_resource_group.sn_rg.location
  version = "12.0"
  administrator_login = "mohab"
  administrator_login_password = "p@ssw0rD!!"

  tags = {
  "environment" = "development"
    }
}
resource "azurerm_sql_database" "sohat_db" {
  name = "sohatdb"
  resource_group_name = azurerm_resource_group.sn_rg.name
  location = azurerm_resource_group.sn_rg.location
  server_name = azurerm_sql_server.sohat_db_server.name
  edition = "Basic"

  tags = {
      "environment" = "development"
  }
}