using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using Newtonsoft.Json;
using NT.Infrastructure.MessageBus.Event;
using RabbitMQ.Client;

namespace NT.Infrastructure.MessageBus.RabbitMq
{
    public class RabbitMqPublisher : IDisposable, IEventBus
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly string _exchangeName;
        private bool _disposed;

        public RabbitMqPublisher(string uri, string exchangeName)
        {
            _exchangeName = exchangeName;
            var factory = new ConnectionFactory { Uri = uri };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _channel.Dispose();
            _connection.Dispose();
            _disposed = true;
        }

        public IObservable<Unit> Publish<T>(T message)
        {
            return Observable.Start(() =>
            {
                // send to queue
                _channel.ExchangeDeclare(_exchangeName, "fanout");
                var json = JsonConvert.SerializeObject(
                    message,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                var bytes = Encoding.UTF8.GetBytes(json);
                _channel.BasicPublish(_exchangeName, "", null, bytes);
            });
        }
    }
}