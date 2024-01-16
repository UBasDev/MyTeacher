using RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLog.WORKER.Models
{
    public class AppSettings
    {
        public RabbitMqSettings RabbitMqSettings { get; set; } = new();
    }
}
