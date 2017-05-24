### Service Discovery (Windows only)

`consul.exe agent -dev`

### Migration

- IdentityServer

`dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/PersistedGrantDb`

`dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/ConfigurationDb`

- Customer Service

`dotnet ef migrations add InitDatabase -c CustomerDbContext`

`dotnet run`

- Order Service

`dotnet ef migrations add InitDatabase -c OrderDbContext`

`dotnet run`

### Login

`root@shoppingcart.com` / `root`

### Services

- Security Service: http://localhost:9999/.well-known/openid-configuration
- API Gateway: http://localhost:8888/swagger
- Discovery Service: http://localhost:8500
- Customer Service: http://localhost:8801
