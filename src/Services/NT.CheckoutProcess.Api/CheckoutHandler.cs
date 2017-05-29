using System;
using System.Reactive;
using System.Reactive.Linq;
using NT.CheckoutProcess.Infrastructure;
using NT.Core.Events;
using NT.Infrastructure.MessageBus.Event;

namespace NT.CheckoutProcess.Api
{
    public class CheckoutHandler : IEventBusHandler<CheckoutEvent>
    {
        private readonly CheckoutWorkflow _workflow;

        public CheckoutHandler(CheckoutWorkflow workflow)
        {
            _workflow = workflow;
        }

        public IObservable<Unit> Handle(CheckoutEvent message)
        {
            _workflow.Checkout(Guid.NewGuid(), message.OrderId).Wait();
            return Observable.Return(new Unit());
        }
    }
}