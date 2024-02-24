using CoreService.Application.Repositories.GenericMongoRepository;
using CoreService.Application.Repositories.GenericRepository;
using CoreService.Domain.Entities.ProfilePicture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories.ProfilePictureRepository
{
    public interface IProfilePictureWriteRepository : IGenericMongoWriteRepository<ProfilePictureEntity>
    {
    }
}
