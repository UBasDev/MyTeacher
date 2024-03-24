using CoreService.Application.Repositories.GenericRepository;
using CoreService.Domain.Entities.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories.RoleRepository
{
    public interface IRoleWriteRepository : IGenericWriteRepository<RoleEntity>
    {
    }
}