### Introduction

The ShoppingCart project is only for the demo in training courses.

### Migrations

- Security Service

```
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/PersistedGrantDb
```

```
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Migrations/ConfigurationDb
```

```
dotnet ef migrations add InitDatabase -c IdentityServerDbContext -o Migrations/IdentityDb
```

```
dotnet run
```

- Customer Service

```
dotnet ef migrations add InitDatabase -c CustomerDbContext
```

```
dotnet run
```

- Order Service

```
dotnet ef migrations add InitDatabase -c OrderDbContext
```

```
dotnet run
```

- Catalog Service

```
dotnet ef migrations add InitDatabase -c CatalogDbContext
```

```
dotnet run
```

- Checkout Process Host

```
dotnet ef migrations add InitDatabase -c CheckoutProcessDbContext
```

```
dotnet run
```

### Service Discovery (Windows only)

```
consul.exe agent -dev
```

### RabbitMQ

- Make sure installing the RabbitMQ for Windows at https://www.rabbitmq.com/install-windows.html
- Enable the management UI at https://www.rabbitmq.com/management.html
- Create the `root` / `root` user
- Create the `shopping_cart` queue and `shopping_cart` vhost (assign the `root` user to this vhost)

### Services

- API Gateway (core): http://localhost:8888/swagger
- Security Service (core): http://localhost:9999/.well-known/openid-configuration (`root@shoppingcart.com` / `root`)
- Customer Service: http://localhost:8801
- Order Service: http://localhost:8802
- Catalog Service: http://localhost:8803
- Checkout Service: http://localhost:8804
- RabbitMQ endpoint (core): http://localhost:15672 (`guest` / `guest`)
- Discovery Service (core): http://localhost:8500

![Discovery Service](https://github.com/thangchung/ShoppingCartDemo/blob/master/docs/ServiceDiscovery.png)

### Saga flow

![Checkout flow](https://github.com/thangchung/ShoppingCartDemo/blob/master/docs/CheckoutProcess.png)
