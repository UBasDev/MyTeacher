using CoreService.Application.Repositories.GenericRepository;
using CoreService.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories.UserRepository
{
    public interface IUserWriteRepository : IGenericWriteRepository<UserEntity>
    {
    }
}
