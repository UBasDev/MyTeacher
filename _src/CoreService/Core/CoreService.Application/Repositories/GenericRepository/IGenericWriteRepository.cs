using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories.GenericRepository
{
    public interface IGenericWriteRepository<TEntity> where TEntity : class
    {
        void InsertSingle(TEntity entity);
        Task InsertSingleAsync(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        void DeleteSingle(TEntity entityToDelete);
        void DeleteSingleById(object id);
        Task DeleteSingleByIdAsync(object id);
        void DeleteRange(IEnumerable<TEntity> entitiesToDelete);
        void UpdateSingle(TEntity entityToUpdate);
        void UpdateRange(IEnumerable<TEntity> entitiesToUpdate);
    }
}
