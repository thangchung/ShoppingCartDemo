using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reflection;

namespace NT.Infrastructure.MessageBus.Event
{
    public class EventConsumer : IEventConsumer
    {
        public EventConsumer(
            IEventSubscriber subscriber,
            IEnumerable<IMessageHandler> eventHandlers)
        {
            Subscriber = subscriber;
            EventHandlers = eventHandlers;
            subscriber.MessageReceived += (sender, e) =>
            {
                if (EventHandlers == null) return;
                foreach (var handler in EventHandlers)
                {
                    var handlerType = handler.GetType();
                    var messageType = e.Message.GetType();
                    var methodInfoQuery =
                        from method in handlerType.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        let parameters = method.GetParameters()
                        where method.Name == "Handle" &&
                              method.ReturnType == typeof (IObservable<Unit>) &&
                              parameters.Length == 1 &&
                              parameters[0].ParameterType == messageType
                        select method;
                    var methodInfo = methodInfoQuery.FirstOrDefault();
                    methodInfo?.Invoke(handler, new[] {e.Message});
                }
            };
        }

        public IEnumerable<IMessageHandler> EventHandlers { get; }

        public IEventSubscriber Subscriber { get; }

        public void Dispose()
        {
            Subscriber.Dispose();
        }
    }
}