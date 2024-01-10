using CoreService.Application.Repositories.GenericRepository;
using CoreService.Domain.Entities.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories.ProfileRepository
{
    public interface IProfileReadRepository : IGenericReadRepository<ProfileEntity>
    {

    }
}
