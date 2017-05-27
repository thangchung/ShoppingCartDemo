using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.CustomerService.Core;
using NT.Infrastructure.AspNetCore;
using NT.OrderService.Core;

namespace NT.WebApi.OrderContext
{
    [Route("api/orders")]
    public class OrderApiController : BaseGatewayController
    {
        public OrderApiController(RestClient restClient) 
            : base(restClient)
        {
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await RestClient.GetAsync<List<Order>>("order_service", "/api/orders");
        }

        [HttpGet("{id}")]
        public async Task<OrderViewModel> Get(Guid id)
        {
            var order = await RestClient.GetAsync<Order>("order_service", $"/api/orders/{id}");
            var customer = await RestClient.GetAsync<Customer>("customer_service", $"/api/customers/{order.CustomerId}");
            var user = await RestClient.GetAsync<UserViewModel>("security_service", $"/api/users/{order.EmployeeId}");
            var details = new List<OrderDetailViewModel>();
            foreach (var orderDetail in order.OrderDetails)
            {
                var product = await RestClient.GetAsync<Product>("catalog_service", $"/api/products/{orderDetail.ProductId}");
                details.Add(new OrderDetailViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductPrice = product.Price,
                    Quantity = orderDetail.Quantity
                });
            }
            
            return new OrderViewModel
            {
                OrderId = order.Id,
                CustomerId = customer.Id,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                EmployeeId = new Guid(user.Id),
                EmployeeName = user.Email,
                OrderDate = order.OrderDate,
                ShipInfoName = order.ShipInfo.Name,
                Address = order.ShipInfo.AddressInfo.Address,
                City = order.ShipInfo.AddressInfo.City,
                Region = order.ShipInfo.AddressInfo.Region,
                PostalCode = order.ShipInfo.AddressInfo.PostalCode,
                Country = order.ShipInfo.AddressInfo.Country,
                OrderDetails = details
            };
        }
    }
}