### Introduction

The ShoppingCart project is only for the demo in training courses.

### System overview

![System Overview](https://github.com/thangchung/ShoppingCartDemo/blob/master/docs/SystemOverview.png)

### [Migrations](https://github.com/thangchung/ShoppingCartDemo/wiki/Migrations)

### Services

- Customer Service: http://localhost:8801
- Order Service: http://localhost:8802
- Catalog Service: http://localhost:8803
- Checkout Service: http://localhost:8804
- Payment Service: http://localhost:8805
- Audit Service (core): http://localhost:8806
- API Gateway (core): http://localhost:8888/swagger
- Security Service (core): http://localhost:9999/.well-known/openid-configuration (`root@shoppingcart.com` / `root`)
- RabbitMQ endpoint (core): http://localhost:15672 (`guest` / `guest`)
- Discovery Service (core): http://localhost:8500

### Saga flow

- The end user submits the checkout 

![Checkout flow 1](https://github.com/thangchung/ShoppingCartDemo/blob/master/docs/CheckoutSaga_1.png)

- The payment gateway accepts the payment request and calls back to Saga

![Checkout flow 2](https://github.com/thangchung/ShoppingCartDemo/blob/master/docs/CheckoutSaga_2.png)

### Service Discovery (Windows only)

```
consul.exe agent -dev
```

![Discovery Service](https://github.com/thangchung/ShoppingCartDemo/blob/master/docs/ServiceDiscovery.png)

### RabbitMQ

- Make sure installing the RabbitMQ for Windows at https://www.rabbitmq.com/install-windows.html
- Enable the management UI at https://www.rabbitmq.com/management.html
- Create the `root` / `root` user
- Create the `shopping_cart` queue and `shopping_cart` vhost (assign the `root` user to this vhost)
