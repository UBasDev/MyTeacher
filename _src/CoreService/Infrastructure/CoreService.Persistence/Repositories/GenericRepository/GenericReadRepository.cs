using CoreService.Application.Contexts;
using CoreService.Application.Repositories.GenericRepository;
using CoreService.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories.GenericRepository
{
    public abstract class GenericReadRepository<TEntity> : IGenericReadRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        protected GenericReadRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate);

        public virtual IQueryable<TEntity> FindByConditionAsNoTracking(Expression<Func<TEntity, bool>> predicate) => _dbSet.AsNoTracking().Where(predicate);

        public virtual IEnumerable<TEntity> GetAll() => _dbSet.ToList();

        public virtual IEnumerable<TEntity> GetAllAsNoTracking() => _dbSet.AsNoTracking().ToList();

        public virtual async Task<IEnumerable<TEntity>> GetAllAsNoTrackingAsync() => await _dbSet.AsNoTracking().ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual TEntity? GetSingleById(object id) => _dbSet.Find(id);

        public virtual TEntity? GetSingleByIdAsNoTracking(object id)
        {
            var entity = _dbSet.Find(id);
            if (entity == null) return null;
            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<TEntity?> GetSingleByIdAsNoTrackingAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return null;
            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<TEntity?> GetSingleByIdAsync(object id) => await _dbSet.FindAsync(id);
    }
}
