using System;
using System.Reactive;
using System.Reactive.Linq;
using NT.Infrastructure.MessageBus.Event;
using NT.OrderService.Core;

namespace NT.WebApi.OrderContext
{
    public class TestHandler : IEventBusHandler<TestedEvent>
    {
        public IObservable<Unit> Handle(TestedEvent message)
        {
            return Observable.Return(new Unit());
        }
    }
}