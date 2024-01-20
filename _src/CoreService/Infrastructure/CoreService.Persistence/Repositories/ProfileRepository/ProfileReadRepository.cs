using CoreService.Application.Contexts;
using CoreService.Application.Repositories.ProfileRepository;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Profile;
using CoreService.Persistence.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories.ProfileRepository
{
    public class ProfileReadRepository(ApplicationDbContext _dbContext) : GenericReadRepository<ProfileEntity, Guid>(_dbContext), IProfileReadRepository
    {
    }
}
