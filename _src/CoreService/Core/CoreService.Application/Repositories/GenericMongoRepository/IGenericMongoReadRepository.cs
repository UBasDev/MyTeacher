﻿
using MongoDb.Concretes;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CoreService.Application.Repositories.GenericMongoRepository
{
    public interface IGenericMongoReadRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllDocumentsAsync(MongoCollectionSettings? collectionSettings = null);
        Task<IEnumerable<TEntity>> GetDocumentsByConditionAsync(Expression<Func<TEntity, bool>> condition, MongoCollectionSettings? collectionSettings = null);
    }
}
