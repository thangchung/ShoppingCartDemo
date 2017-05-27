using System;
using System.Reactive;

namespace NT.Infrastructure.MessageBus.Event
{
    public interface IEventSubscriber : IDisposable
    {
        IObservable<Unit> Subscribe();
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}