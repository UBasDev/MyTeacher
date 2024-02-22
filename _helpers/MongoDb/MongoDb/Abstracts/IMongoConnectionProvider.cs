using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Abstracts
{
    public interface IMongoConnectionProvider
    {
        public IMongoDatabase _mongoDb { get; }
    }
}
