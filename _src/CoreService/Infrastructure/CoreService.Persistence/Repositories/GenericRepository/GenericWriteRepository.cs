using CoreService.Application.Contexts;
using CoreService.Application.Repositories.GenericRepository;
using CoreService.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories.GenericRepository
{
    public abstract class GenericWriteRepository<TEntity> : IGenericWriteRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        protected GenericWriteRepository(ApplicationDbContext dbContext)
        {
            _dbSet = dbContext.Set<TEntity>();
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entitiesToDelete) => _dbSet.RemoveRange(entitiesToDelete);

        public virtual void DeleteSingle(TEntity entityToDelete) => _dbSet.Remove(entityToDelete);

        public virtual void DeleteSingleById(object id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null) _dbSet.Remove(entity);
        }

        public virtual async Task DeleteSingleByIdAsync(object id, CancellationToken cancellationToken)
        {
            var entity = await _dbSet.FindAsync(id, cancellationToken);
            if (entity != null) _dbSet.Remove(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken) => await _dbSet.AddRangeAsync(entities, cancellationToken);

        public virtual void InsertSingle(TEntity entity) => _dbSet.Add(entity);

        public async Task InsertSingleAsync(TEntity entity, CancellationToken cancellationToken) => await _dbSet.AddAsync(entity, cancellationToken);

        public virtual void UpdateRange(IEnumerable<TEntity> entitiesToUpdate) => _dbSet.UpdateRange(entitiesToUpdate);

        public virtual void UpdateSingle(TEntity entityToUpdate) => _dbSet.Update(entityToUpdate);
    }
}