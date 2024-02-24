using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Models
{
    public class MongoDbSettings
    {
        public string AuthDbName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
        public UInt16 Port { get; set; }
        public UInt16 ConnectTimeout { get; set; }
        public UInt16 QueueTimeout { get; set; }
        public bool UseSSl { get; set; }
        public byte MinConnectionPoolSize { get; set; }
        public byte MaxConnectionPoolSize { get; set; }
        public string DatabaseName { get; set; }
        public MongoDbSslSettings MongoDbSslSettings { get; set; }
    }
    public class MongoDbSslSettings
    {
        public bool CheckCertificateRevocation { get; set; }
    }
}
