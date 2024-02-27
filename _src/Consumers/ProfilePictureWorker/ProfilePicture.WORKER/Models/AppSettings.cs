using MongoDb.Models;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilePicture.WORKER.Models
{
    public class AppSettings
    {
        public MongoDbSettings MongoDbSettings { get; set; } = new();
        public EventBusSettings EventBusSettings { get; set; } = new();
    }
}
