using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Core;
using NT.Core.Results;
using NT.Core.SharedKernel;
using NT.Infrastructure;
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

        [HttpPost]
        public async Task<Order> Post([FromBody] OrderCreationViewModel viewModel)
        {
            var order = new Order
            {
                CustomerId = viewModel.CustomerId,
                EmployeeId = viewModel.EmployeeId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.New,
                OrderDetails = viewModel.Products,
                ShipInfo = new ShipInfo(
                    Guid.NewGuid(),
                    viewModel.ShipInfo.Name,
                    new AddressInfo(
                        Guid.NewGuid(),
                        viewModel.ShipInfo.Address,
                        viewModel.ShipInfo.City,
                        viewModel.ShipInfo.Region,
                        viewModel.ShipInfo.PostalCode,
                        viewModel.ShipInfo.Country))
            };
            return await _genericOrderRepository.AddAsync(order);
        }

        [HttpPut("{orderId}/status/{status}")]
        public async Task<SagaResult> Put(Guid orderId, OrderStatus status)
        {
            var order = await _genericOrderRepository.GetByIdAsync(orderId);
            if (order == null)
                return await Task.FromResult(new SagaResult {Succeed = false});

            order.OrderStatus = status;
            await _genericOrderRepository.UpdateAsync(order);
            return await Task.FromResult(new SagaResult {Succeed = true});
        }

        [HttpPut("{orderId}/update-saga/{sagaId}")]
        public async Task<SagaResult> UpdateSagaInfo(Guid orderId, Guid sagaId)
        {
            var order = await _genericOrderRepository.GetByIdAsync(orderId);
            if (order == null)
                return await Task.FromResult(new SagaResult {Succeed = false});

            order.SagaId = sagaId;
            await _genericOrderRepository.UpdateAsync(order);
            return await Task.FromResult(new SagaResult {Succeed = true});
        }
    }

    public class OrderCreationViewModel
    {
        public List<OrderDetail> Products { get; set; }
        public ShipInfoViewModel ShipInfo { get; set; }
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
    }

    public class ShipInfoViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}