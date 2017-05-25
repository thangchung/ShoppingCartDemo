using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.CatalogService.Core;
using NT.CustomerService.Core;
using NT.Infrastructure;
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

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}