using CoreService.Application.Repositories.GenericMongoRepository;
using CoreService.Domain.Entities.ProfilePicture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories.ProfilePicture
{
    public interface IProfilePictureReadRepository : IGenericMongoReadRepository<ProfilePictureEntity>
    {
    }
}
