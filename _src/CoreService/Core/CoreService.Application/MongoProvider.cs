using CoreService.Domain.Entities.ProfilePicture;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CoreService.Application
{
    public class MongoProvider : IMongoProvider
    {
        public IMongoDatabase _mongoDb { get; private set; }
        public MongoProvider() {
            var client = new MongoClient("mongodb://root:password1@localhost:27017");
            _mongoDb = client.GetDatabase("MyTeacher-Core");
        }
        
        public async Task Test1()
        {
            var x1 = _mongoDb.GetCollection<ProfilePicture>("ProfilePictures");
            await x1.InsertOneAsync(ProfilePicture.CreateNewProfilePicture(new Random().Next(0, 1000).ToString(), "photoPath1", "photoExt1", (UInt32)new Random().Next(0, 1000)));
            using var x2 = (await x1.FindAsync(_ => true)).ToListAsync();
        }
    }
}
