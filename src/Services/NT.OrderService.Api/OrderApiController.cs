using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Core;
using NT.Core.Results;
using NT.OrderService.Core;

namespace NT.OrderService.Api
{
    [Route("api/orders")]
    public class OrderApiController : Controller
    {
        private readonly IRepository<Order> _genericOrderRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderApiController(
            IRepository<Order> genericOrderRepository,
            IOrderRepository orderRepository)
        {
            _genericOrderRepository = genericOrderRepository;
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

        [HttpPut("{orderId}/status/{status}")]
        public async Task<SagaResult> Put(Guid orderId, OrderStatus status)
        {
            var order = await _genericOrderRepository.GetByIdAsync(orderId);
            if(order == null)
                return await Task.FromResult(new SagaResult { Succeed = false });

            order.OrderStatus = status;
            await _genericOrderRepository.UpdateAsync(order);
            return await Task.FromResult(new SagaResult { Succeed = true });
        }

        [HttpPut("{orderId}/update-saga/{sagaId}")]
        public async Task<SagaResult> UpdateSagaInfo(Guid orderId, Guid sagaId)
        {
            var order = await _genericOrderRepository.GetByIdAsync(orderId);
            if (order == null)
                return await Task.FromResult(new SagaResult { Succeed = false });

            order.SagaId = sagaId;
            await _genericOrderRepository.UpdateAsync(order);
            return await Task.FromResult(new SagaResult { Succeed = true });
        }
    }
}