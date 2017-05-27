using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Infrastructure.MessageBus.Event;
using NT.OrderService.Core;

namespace NT.OrderService.Api
{
    [Route("api/orders")]
    public class OrderQueryController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventBus _messageBus;

        public OrderQueryController(IOrderRepository orderRepository, IEventBus messageBus)
        {
            _orderRepository = orderRepository;
            _messageBus = messageBus;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _orderRepository.GetFullOrders();
        }

        [HttpGet("{id}")]
        public async Task<Order> Get(Guid id)
        {
            _messageBus.Publish(new TestedEvent
            {
                Message = "Hello from Order Service"
            });

            return await _orderRepository.GetFullOrder(id);
        }
    }
}