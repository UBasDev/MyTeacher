using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Reflection;

namespace FileLogWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    VirtualHost = "/",
                    ClientProvidedName = "Rabbit Receiver App1"
                };

                //factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");

                var conn = factory.CreateConnection();

                var channel = conn.CreateModel();

                const string exchangeName1 = "DemoExchange1";
                const string routingKey1 = "demo-routing-key1";
                const string queueName1 = "DemoQueue1";

                channel.ExchangeDeclare(exchangeName1, ExchangeType.Direct);

                channel.QueueDeclare(
                    queue: queueName1,

                    durable: true, //Eðer true alýrsa, bu message broker restart edilse bile bu queue korunacaktýr ve silinmeyecektir

                    exclusive: false, //

                    autoDelete: false, //Eðer true alýrsa, bu queue'nun last consumerý unsubscribe edildiði zaman, bu queue da silinecektir.

                    arguments: null
                );

                channel.QueueBind(queue: queueName1, exchange: exchangeName1, routingKey: routingKey1, arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, eventArgs) =>
                {
                    var isError = false;

                    var stream = CreateFileStream();

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

                        var body = eventArgs.Body.ToArray(); //Queue'den gelen datayý çekiyoruz

                        var messageToLog = GenerateInfoMessage(this.GetType().Name, Encoding.UTF8.GetString(body));

                        stream.WriteLine(messageToLog);

                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        string errorMessageToLog = GenerateErrorMessage(this.GetType().Name, ex.Message);
                        stream.WriteLine(errorMessageToLog);
                    }

                    FlushStreamAndClose(stream);

                    if (!isError) channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);

                    else channel.BasicNack(deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: false);
                };

                string consumerTag1 = channel.BasicConsume(queue: queueName1, autoAck: false, consumer: consumer);

                //channel.BasicCancel(consumerTag: consumerTag1); //Burasý çalýþýrsa bu consumer, bu queueden unsubscribe oluyor ve listen etmeyi BIRAKIYOR.

                //channel.Close();

                //conn.Close();

            }
            catch (Exception ex)
            {
                var stream = CreateFileStream();
                string errorMessageToLog = GenerateErrorMessage(this.GetType().Name, ex.Message);
                stream.WriteLine(errorMessageToLog);
                FlushStreamAndClose(stream);
            }
        }
        private static StreamWriter CreateFileStream()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

            string currentMonth = currentDate.Month < 10 ? "0" + currentDate.Month : currentDate.Month.ToString();
            string currentDay = currentDate.Day < 10 ? "0" + currentDate.Day : currentDate.Day.ToString();

            return new StreamWriter(path: Path.Combine(Directory.GetCurrentDirectory(), "LogFiles", $"{currentDay}_{currentMonth}_{currentDate.Year}_logs.txt"), append: true, encoding: Encoding.UTF8, bufferSize: 1024);
        }
        private static void FlushStreamAndClose(StreamWriter stream)
        {
            stream.Flush();
            stream.Dispose();
            stream.Close();
        }
        private static string GenerateErrorMessage(string fileName, string errorMessage)
        {
            return $"ERROR => DATETIME: {DateTime.UtcNow} - FILE: {fileName}.cs - CONTENT: {errorMessage}";
        }
        private static string GenerateInfoMessage(string fileName, string infoMessage)
        {
            return $"INFO => DATETIME: {DateTime.UtcNow} - FILE: {fileName}.cs - CONTENT: {infoMessage}";
        }
    }
}
