using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.Core.Events;
using NT.Infrastructure.AspNetCore;
using NT.Infrastructure.MessageBus.Event;

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
        public Task<bool> Post(Guid orderId)
        {
            _messageBus.Publish(new CheckoutEvent(orderId));
            return Task.FromResult(true);
        }
    }
}