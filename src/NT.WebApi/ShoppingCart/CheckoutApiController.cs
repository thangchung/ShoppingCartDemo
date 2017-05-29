using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Core.Events;
using NT.Infrastructure.AspNetCore;
using NT.Infrastructure.MessageBus.Event;
using NT.OrderService.Core;

namespace NT.WebApi.ShoppingCart
{
    [Route("api/checkout")]
    public class CheckoutApiController : BaseGatewayController
    {
        private readonly IEventBus _messageBus;

        public CheckoutApiController(RestClient restClient, IEventBus messageBus)
            : base(restClient)
        {
            _messageBus = messageBus;
        }

        [HttpPost]
        public async Task<string> Post(Guid orderId)
        {
            var order = await RestClient.GetAsync<Order>("order_service", $"/api/orders/{orderId}");
            if (order.OrderStatus == OrderStatus.Processing || order.OrderStatus == OrderStatus.Paid)
                return await Task.FromResult($"Order[#{orderId}] was in Processing or Paid status.");
            _messageBus.Publish(new CheckoutEvent(orderId));
            return await Task.FromResult($"Order[#{orderId}] is processing...");
        }
    }
}