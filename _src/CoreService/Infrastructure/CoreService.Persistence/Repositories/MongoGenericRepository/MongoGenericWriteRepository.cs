using CoreService.Application.Repositories.GenericMongoRepository;
using CoreService.Domain.Entities.Common;
using Microsoft.Extensions.Logging;
using MongoDb.Concretes;
using MongoDb.Models;
using MongoDB.Driver;

namespace CoreService.Persistence.Repositories.MongoGenericRepository
{
    public class MongoGenericWriteRepository<TEntity> : MongoConnectionProvider, IGenericMongoWriteRepository<TEntity> where TEntity :class
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

        public async Task<(bool isSuccessful, string? errorMessage)> UpdateMultipleDocumentsAsync(IEnumerable<TEntity> documents)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool isSuccessful, string? errorMessage)> UpdateSingleDocumentAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            /*
            var filter1 = Builders<TEntity>.Filter.Eq(p => p.Id, "sada");
            var update1 = Builders<TEntity>.Update.Set(p => p.Id, "sa");
            */
            try
            {
                var updateResult = await _collection.UpdateOneAsync(filter, update);
                if(updateResult.ModifiedCount > 0 && updateResult.MatchedCount > 0) return (true, null);
                return (false, null);
            }catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
