﻿using CoreService.Application.Repositories.GenericMongoRepository;
using CoreService.Domain.Entities.Common;
using MongoDb.Concretes;
using MongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories.MongoGenericRepository
{
    public class MongoGenericReadRepository<TEntity, TId>(MongoDbSettings mongoDbSettings) : MongoConnectionProvider(mongoDbSettings), IGenericMongoReadRepository<TEntity> where TEntity : BaseEntity<TId>
    {
        public async Task<IEnumerable<TEntity>> GetAllDocumentsAsync(string collectionName, MongoCollectionSettings? collectionSettings = null)
        {
            var collectionData = _mongoDb.GetCollection<TEntity>(collectionName, collectionSettings ?? new MongoCollectionSettings() { });
            return await (await collectionData.FindAsync(_ => true)).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetDocumentsByConditionAsync(string collectionName, Expression<Func<TEntity, bool>> condition, MongoCollectionSettings? collectionSettings = null)
        {
            var collectionData = _mongoDb.GetCollection<TEntity>(collectionName, collectionSettings ?? new MongoCollectionSettings() { });
            return await (await collectionData.FindAsync(condition)).ToListAsync();
        }
    }
}