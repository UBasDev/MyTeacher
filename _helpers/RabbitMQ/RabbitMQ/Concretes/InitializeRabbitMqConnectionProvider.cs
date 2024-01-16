using RabbitMQ.Abstracts;
using RabbitMQ.Client;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Concretes
{
    public class InitializeRabbitMqConnectionProvider : IInitializeRabbitMqConnectionProvider
    {
        protected readonly IConnection _connection;
        protected readonly IModel _channel;
        protected readonly RabbitMqSettings _rabbitMqSettings;
        protected InitializeRabbitMqConnectionProvider(RabbitMqSettings rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;

            var factory = CreateConnectionFactory();
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            InitializeRabbitMqConnection();
        }
        private ConnectionFactory CreateConnectionFactory()
        {
            return new ConnectionFactory()
            {
                HostName = _rabbitMqSettings.HostName,
                UserName = _rabbitMqSettings.Username,
                Password = _rabbitMqSettings.Password,
                VirtualHost = _rabbitMqSettings.VirtualHost,
                ClientProvidedName = _rabbitMqSettings.ClientProvidedName
            };
        }
        private void InitializeRabbitMqConnection()
        {
            string exchangeName1 = _rabbitMqSettings.ExchangeName;
            string queueName1 = _rabbitMqSettings.QueueName;
            string routingKey1 = _rabbitMqSettings.RoutingKey;

            _channel.ExchangeDeclare(exchangeName1, ExchangeType.Direct);

            _channel.QueueDeclare(
                queue: queueName1,
                durable: _rabbitMqSettings.IsDurable, //Eğer true alırsa, bu message broker restart edilse bile bu queue korunacaktır ve silinmeyecektir
                exclusive: _rabbitMqSettings.IsExclusive,
                autoDelete: _rabbitMqSettings.IsAutoDelete, //Eğer true alırsa, bu queue'nun last consumerı unsubscribe edildiği zaman, bu queue da silinecektir.
                arguments: _rabbitMqSettings?.Arguments ?? null
            );

            _channel.QueueBind(queue: queueName1, exchange: exchangeName1, routingKey: routingKey1, arguments: null);

            _channel.BasicQos(prefetchSize: _rabbitMqSettings.PrefetchSize, prefetchCount: _rabbitMqSettings.PrefetchCount, global: _rabbitMqSettings.IsGlobal);
        }
        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _channel.Close();
                _connection.Close();
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
