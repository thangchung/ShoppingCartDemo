using System;
using System.Reactive;
using System.Reactive.Linq;
using NT.Infrastructure.MessageBus.Event;
using NT.OrderService.Core;

namespace NT.CheckoutProcess.Infrastructure
{
    public class SecondTestHandler : IEventBusHandler<TestedEvent>
    {
        public IObservable<Unit> Handle(TestedEvent message)
        {
            return Observable.Return(new Unit());
        }
    }
}