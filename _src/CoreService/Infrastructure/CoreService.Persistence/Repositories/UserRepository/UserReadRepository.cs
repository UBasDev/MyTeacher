using CoreService.Application.Contexts;
using CoreService.Application.Repositories.ProfileRepository;
using CoreService.Application.Repositories.UserRepository;
using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.User;
using CoreService.Persistence.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories.UserRepository
{
    public class UserReadRepository(ApplicationDbContext _dbContext) : GenericReadRepository<UserEntity>(_dbContext), IUserReadRepository
    {
    }
}
