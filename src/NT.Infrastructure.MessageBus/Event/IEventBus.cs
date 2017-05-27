using System;
using System.Reactive;

namespace NT.Infrastructure.MessageBus.Event
{
    public interface IEventBus
    {
        IObservable<Unit> Publish<T>(T message);
    }
}