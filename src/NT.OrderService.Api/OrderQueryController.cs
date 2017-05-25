using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.OrderService.Core;

namespace NT.OrderService.Api
{
    [Route("api/orders")]
    public class OrderQueryController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        
        public OrderQueryController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _orderRepository.GetFullOrders();
        }

        [HttpGet("{id}")]
        public async Task<Order> Get(Guid id)
        {
            return await _orderRepository.GetFullOrder(id);
        }
    }
}