### Introduction

The ShoppingCart project only for demo in training courses.

### Migrations

- Sercurity Service

`dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/PersistedGrantDb`

`dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/ConfigurationDb`

`dotnet ef migrations add InitDatabase -c IdentityServerDbContext -o Migrations/IdentityDb`

`dotnet run`

- Customer Service

`dotnet ef migrations add InitDatabase -c CustomerDbContext`

`dotnet run`

- Order Service

`dotnet ef migrations add InitDatabase -c OrderDbContext`

`dotnet run`

- Catalog Service

`dotnet ef migrations add InitDatabase -c CatalogDbContext`

`dotnet run`

### Login

`root@shoppingcart.com` / `root`

### Service Discovery (Windows only)

`consul.exe agent -dev`

### RabbitMQ

- Make sure installing the RabbitMQ for Windows at https://www.rabbitmq.com/install-windows.html
- Enable the management UI at https://www.rabbitmq.com/management.html
- Create the `root` / `root` user
- Create the `shopping_cart` queue and `shopping_cart` vhost (assign the `root` user to this vhost)

### Services

- API Gateway: http://localhost:8888/swagger
- Security Service: http://localhost:9999/.well-known/openid-configuration
- Customer Service: http://localhost:8801
- Order Service: http://localhost:8802
- Catalog Service: http://localhost:8803
- RabbitMQ endpoint: http://localhost:15672 (`guest` / `guest`)
- Discovery Service: http://localhost:8500

![Discovery Service](https://github.com/thangchung/ShoppingCartDemo/blob/master/docs/ServiceDiscovery.png)
