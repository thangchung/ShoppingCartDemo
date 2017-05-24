using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Core;
using NT.OrderService.Core;

namespace NT.OrderService.Api
{
    [Route("api/orders")]
    public class OrderQueryController : Controller
    {
        private readonly IRepository<Order> _genericOrderRepository;

        public OrderQueryController(IRepository<Order> genericOrderRepository)
        {
            _genericOrderRepository = genericOrderRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _genericOrderRepository.ListAsync(
                new ISpecification<Order>[]
                {
                    new OrderWithOrderDetailSpec(),
                    new OrderWithShipInfoSpec()
                });
        }

        [HttpGet("{id}")]
        public async Task<Order> Get(Guid id)
        {
            return await _genericOrderRepository.GetByIdAsync(
                id,
                new ISpecification<Order>[]
                {
                    new OrderWithOrderDetailSpec(),
                    new OrderWithShipInfoSpec()
                });
        }
    }
}