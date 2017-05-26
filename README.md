### Introduction

The ShoppingCart project only for demo in training courses.

### Migration

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

### Services

- Security Service: http://localhost:9999/.well-known/openid-configuration
- API Gateway: http://localhost:8888/swagger
- Discovery Service: http://localhost:8500

![Discovery Service](https://github.com/thangchung/ShoppingCartDemo/blob/master/docs/ServiceDiscovery.png)

- Customer Service: http://localhost:8801
- Order Service: http://localhost:8802
- Catalog Service: http://localhost:8803
