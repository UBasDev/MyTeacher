using CoreService.Application.Repositories.ProfileRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
        IProfileReadRepository ProfileReadRepository { get; }
        IProfileWriteRepository ProfileWriteRepository { get; }
    }
}
