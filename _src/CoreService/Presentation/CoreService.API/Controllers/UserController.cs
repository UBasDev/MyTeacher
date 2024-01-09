using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateSingleUser()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                ClientProvidedName = "Rabbit Sender App1"
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

                durable: true, //Eğer true alırsa, bu message broker restart edilse bile bu queue korunacaktır ve silinmeyecektir

                exclusive: false, //

                autoDelete: false, //Eğer true alırsa, bu queue'nun last consumerı unsubscribe edildiği zaman, bu queue da silinecektir.

                arguments: null
            );

            channel.QueueBind(queue: queueName1, exchange: exchangeName1, routingKey: routingKey1, arguments: null);

            //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var jsonString = JsonSerializer.Serialize<string>(value: "Merhabalar, bu mesajı queueye gönderiyorum.", options: new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            });

            var body = Encoding.UTF8.GetBytes("Merhabalar, bu mesajı queueye gönderiyorum.");

            channel.BasicPublish(exchange: exchangeName1, routingKey: routingKey1, basicProperties: null, body: body);

            channel.Close();

            conn.Close();
            return Ok();
        }
    }
}
