using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories.GenericMongoRepository
{
    public interface IGenericMongoWriteRepository<TEntity>
    {
        Task<(bool isSuccessful, string? errorMessage)> CreateSingleDocumentAsync(TEntity document);
        Task<(bool isSuccessful, string? errorMessage)> CreateMultipleDocumentsAsync(IEnumerable<TEntity> documents);
    }
}
