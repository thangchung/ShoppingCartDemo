using System;
using System.Reactive;
using System.Reactive.Linq;
using NT.CheckoutProcess.Infrastructure;
using NT.Core.Events;
using NT.Infrastructure.MessageBus.Event;

namespace NT.CheckoutProcess.Api
{
    public class CheckoutHandler : IEventBusHandler<CheckoutEvent>, IEventBusHandler<PaymentAcceptedEvent>
    {
        private readonly CheckoutSaga _workflow;

        public CheckoutHandler(CheckoutSaga workflow)
        {
            _workflow = workflow;
        }

        public IObservable<Unit> Handle(CheckoutEvent message)
        {
            _workflow.Checkout(Guid.NewGuid(), message.OrderId).Wait();
            return Observable.Return(new Unit());
        }

        public IObservable<Unit> Handle(PaymentAcceptedEvent message)
        {
            _workflow.PaymentAccepted(message.SagaId).Wait();
            return Observable.Return(new Unit());
        }
    }
}