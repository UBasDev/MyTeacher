using MongoDb.Models;
using MyTeacher.Helper.Models;
using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Models
{
    public class AppSettings
    {
        public string DatabaseConnectionUrl { get; set; } = string.Empty;
        public JwtTokenSettings JwtTokenSettings { get; set; }
        public MongoDbSettings MongoDbSettings { get; set; }
        public EventBusSettings EventBusSettings { get; set; }
    }
}
