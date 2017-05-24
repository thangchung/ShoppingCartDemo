using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Infrastructure;
using NT.OrderService.Core;

namespace NT.WebApi.OrderContext
{
    [Route("api/orders")]
    // [Authorize]
    public class OrderApiController : Controller
    {
        private readonly RestClient _restClient;

        public OrderApiController(RestClient restClient)
        {
            _restClient = restClient;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _restClient.GetAsync<List<Order>>("order_service", "/api/orders");
        }

        [HttpGet("{id}")]
        public async Task<Order> Get(Guid id)
        {
            return await _restClient.GetAsync<Order>("order_service", $"/api/orders/{id}");
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