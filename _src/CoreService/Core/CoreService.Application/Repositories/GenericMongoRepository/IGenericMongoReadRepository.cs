
using MongoDb.Concretes;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CoreService.Application.Repositories.GenericMongoRepository
{
    public interface IGenericMongoReadRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllDocumentsAsync(string collectionName, MongoCollectionSettings? collectionSettings = null);
        Task<IEnumerable<TEntity>> GetDocumentsByConditionAsync(string collectionName, Expression<Func<TEntity, bool>> condition, MongoCollectionSettings? collectionSettings = null);
    }
}
