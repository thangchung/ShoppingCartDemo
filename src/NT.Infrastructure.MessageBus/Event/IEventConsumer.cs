using System;
using System.Collections.Generic;

namespace NT.Infrastructure.MessageBus.Event
{
    public interface IEventConsumer : IDisposable
    {
        IEventSubscriber Subscriber { get; }
        IEnumerable<IMessageHandler> EventHandlers { get; }
    }
}