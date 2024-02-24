using CoreService.Application.Repositories.GenericMongoRepository;
using CoreService.Domain.Entities.Common;
using Microsoft.Extensions.Logging;
using MongoDb.Concretes;
using MongoDb.Models;
using MongoDB.Driver;

namespace CoreService.Persistence.Repositories.MongoGenericRepository
{
    public class MongoGenericWriteRepository<TEntity, TId> : MongoConnectionProvider, IGenericMongoWriteRepository<TEntity> where TEntity : BaseEntity<TId>
    {
        private readonly IMongoCollection<TEntity> _collection;
        public MongoGenericWriteRepository(MongoDbSettings mongoDbSettings, string collectionName) : base(mongoDbSettings)
        {
            _collection = _mongoDb.GetCollection<TEntity>(collectionName);
        }
        public async Task<(bool isSuccessful, string? errorMessage)> CreateMultipleDocumentsAsync(IEnumerable<TEntity> documents)
        {
            try
            {
                await _collection.InsertManyAsync(documents);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool isSuccessful, string? errorMessage)> CreateSingleDocumentAsync(TEntity document)
        {
            try
            {
                await _collection.InsertOneAsync(document);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
