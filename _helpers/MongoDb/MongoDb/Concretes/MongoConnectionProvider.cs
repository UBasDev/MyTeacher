using MongoDb.Abstracts;
using MongoDb.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Concretes
{
    public class MongoConnectionProvider : IMongoConnectionProvider
    {
        public IMongoDatabase _mongoDb { get; private set; }
        public MongoConnectionProvider(MongoDbSettings mongoDbSettings)
        {
            var mongoClientSettings = new MongoClientSettings()
            {
                Credential = MongoCredential.CreateCredential(mongoDbSettings.AuthDbName, mongoDbSettings.Username, mongoDbSettings.Password),
                Server = new MongoServerAddress(mongoDbSettings.Hostname, mongoDbSettings.Port),
                ConnectTimeout = TimeSpan.FromSeconds(mongoDbSettings.ConnectTimeout), //APInin MongoDb ile ilk establish edildiği anda kaç saniye boyunca establish edilemezse timeout vereceğini set ederiz.
                //SocketTimeout = TimeSpan.FromSeconds(30),
                //MaxConnectionIdleTime = TimeSpan.FromMinutes(2),
                //MaxConnectionLifeTime = TimeSpan.FromMinutes(10),
                WaitQueueTimeout = TimeSpan.FromSeconds(mongoDbSettings.QueueTimeout), //MongoDbdeki connection pooldan bir connectionın kullanılabilir hale gelmesi için bir threadin bekleyeceği süreyi set ederiz.
                UseSsl = mongoDbSettings.UseSSl,
                MinConnectionPoolSize = mongoDbSettings.MinConnectionPoolSize, //MongoDbdeki minimum connection poolu set ederiz.
                MaxConnectionPoolSize = mongoDbSettings.MaxConnectionPoolSize, //MongoDbdeki maximum connection poolu set ederiz.
                SslSettings = new SslSettings
                {
                    CheckCertificateRevocation = mongoDbSettings.MongoDbSslSettings.CheckCertificateRevocation,
                    EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                    ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true
                }
            };
            var client = new MongoClient(mongoClientSettings);
            //var client = new MongoClient("mongodb://root:password1@localhost:27017");
            _mongoDb = client.GetDatabase(mongoDbSettings.DatabaseName);
        }
        /*
        public async Task<List<T>> GetAllDocumentsAsync(string collectionName)
        {
            var collectionData = _mongoDb.GetCollection<T>(collectionName, new MongoCollectionSettings() { });
            return await (await collectionData.FindAsync(_ => true)).ToListAsync();
        }

        public async Task<List<T>> GetDocumentsByConditionAsync(string collectionName, Expression<Func<T, bool>> condition)
        {
            var collectionData = _mongoDb.GetCollection<T>(collectionName, new MongoCollectionSettings() { });
            return await (await collectionData.FindAsync(condition)).ToListAsync();
        }

        public async Task<T?> GetSingleDocumentByConditionAsync(string collectionName, Expression<Func<T, bool>> condition)
        {
            var collectionData = _mongoDb.GetCollection<T>(collectionName, new MongoCollectionSettings() { });
            return await (await collectionData.FindAsync(condition)).FirstOrDefaultAsync();
        }
        */
        /*
        public async Task Test1()
        {
            var x1 = _mongoDb.GetCollection<ProfilePicture>("ProfilePictures");

            await x1.InsertOneAsync(ProfilePicture.CreateNewProfilePicture(new Random().Next(0, 1000).ToString(), "photoPath1", "photoExt1", (UInt32)new Random().Next(0, 1000)));

            using var x2 = (await x1.FindAsync(_ => true)).ToListAsync();
        }
        */
    }
}
