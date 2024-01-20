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
    public class RabbitMqPublisherService(RabbitMqSettings rabbitMqSettings) : InitializeRabbitMqConnectionProvider(rabbitMqSettings), IRabbitMqPublisherService
    {
        public void PublishMessageSync(string message)
        {
            /*
            var jsonString = JsonConvert.SerializeObject(value: "Merhabalar, bu mesajı queueye gönderiyorum.", settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            */
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "DemoExchange1", routingKey: "demo-routing-key1", basicProperties: null, body: body);
        }

        public async Task PublishMessageAsync(string message)
        {
            /*
            var jsonString = JsonConvert.SerializeObject(value: "Merhabalar, bu mesajı queueye gönderiyorum.", settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            */
            await Task.Run(() =>
            {
                var body = Encoding.UTF8.GetBytes(message);
                var properties = _channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                _channel.BasicPublish(exchange: _rabbitMqSettings.ExchangeName, routingKey: _rabbitMqSettings.RoutingKey, basicProperties: properties, body: body);
            });
        }

        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _channel.Close();
                _connection.Close();
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
