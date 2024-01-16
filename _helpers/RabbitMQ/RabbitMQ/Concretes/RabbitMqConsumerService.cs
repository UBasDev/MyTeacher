using RabbitMQ.Abstracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitMQ.Concretes
{
    public class RabbitMqConsumerService(RabbitMqSettings rabbitMqSettings) : InitializeRabbitMqConnectionProvider(rabbitMqSettings), IRabbitMqConsumerService
    {
        public async Task ConsumeAllAsync(Func<string, Task> @event)
        {

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var isError = false;

                try
                {
                    /*
                    var x1 = eventArgs.Body;
                    var x2 = eventArgs.Body.Length;
                    var x3 = eventArgs.Body.Span;
                    var x4 = eventArgs.RoutingKey;
                    var x5 = eventArgs.DeliveryTag;
                    var x6 = eventArgs.Exchange;
                    var x7 = eventArgs.ConsumerTag;
                    */

                    var body = eventArgs.Body.ToArray(); //Queue'den gelen datayı çekiyoruz

                    var messageFromQueue = Encoding.UTF8.GetString(body);
                    await @event(messageFromQueue);

                    //Console.WriteLine($"Message received from queue: {messageFromQueue}");
                }
                catch (Exception ex)
                {
                    isError = true;
                    Console.WriteLine(ex.Message);
                }

                if (!isError) _channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);

                else _channel.BasicNack(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: false);
            };

            string consumerTag1 = _channel.BasicConsume(queue: _rabbitMqSettings.QueueName, autoAck: false, consumer: consumer);

            //channel.BasicCancel(consumerTag: consumerTag1); //Burası çalışırsa bu consumer, bu queueden unsubscribe oluyor ve listen etmeyi BIRAKIYOR.
        }

        public void ConsumeAllSync(Action<string> @event)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                var isError = false;

                try
                {
                    /*
                    var x1 = eventArgs.Body;
                    var x2 = eventArgs.Body.Length;
                    var x3 = eventArgs.Body.Span;
                    var x4 = eventArgs.RoutingKey;
                    var x5 = eventArgs.DeliveryTag;
                    var x6 = eventArgs.Exchange;
                    var x7 = eventArgs.ConsumerTag;
                    */

                    var body = eventArgs.Body.ToArray(); //Queue'den gelen datayı çekiyoruz

                    var messageFromQueue = Encoding.UTF8.GetString(body);
                    @event(messageFromQueue);

                    //Console.WriteLine($"Message received from queue: {messageFromQueue}");
                }
                catch (Exception ex)
                {
                    isError = true;
                    Console.WriteLine(ex.Message);
                }

                if (!isError) _channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);

                else _channel.BasicNack(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: false);
            };

            string consumerTag1 = _channel.BasicConsume(queue: _rabbitMqSettings.QueueName, autoAck: false, consumer: consumer);

            //channel.BasicCancel(consumerTag: consumerTag1); //Burası çalışırsa bu consumer, bu queueden unsubscribe oluyor ve listen etmeyi BIRAKIYOR.
        }
    }
}
