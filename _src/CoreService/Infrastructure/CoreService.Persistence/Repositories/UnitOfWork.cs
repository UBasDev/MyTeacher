using CoreService.Application.Contexts;
using CoreService.Application.Repositories;
using CoreService.Application.Repositories.ProfileRepository;
using CoreService.Persistence.Repositories.ProfileRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IProfileReadRepository _profileReadRepository;
        private IProfileWriteRepository _profileWriteRepository;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IProfileReadRepository ProfileReadRepository
        {
            get
            {
                _profileReadRepository = new ProfileReadRepository(_dbContext);
                return _profileReadRepository;
            }
        }

        public IProfileWriteRepository ProfileWriteRepository
        {
            get
            {
                _profileWriteRepository = new ProfileWriteRepository(_dbContext);
                return _profileWriteRepository;
            }
        }

        public void SaveChanges() => _dbContext.SaveChanges();

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }
}
