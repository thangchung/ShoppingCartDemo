using System;
using System.Reactive;
using NT.Core;

namespace NT.Infrastructure.MessageBus.Event
{
    public interface IEventBusHandler<in TMessage> : IMessageHandler
        where TMessage : IMessage
    {
        IObservable<Unit> Handle(TMessage message);
    }
}