using CoreService.Application.Repositories.ProfilePicture;
using CoreService.Application.Repositories.ProfileRepository;
using CoreService.Application.Repositories.UserRepository;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        Task SaveChangesAsync();
        IProfileReadRepository ProfileReadRepository { get; }
        IProfileWriteRepository ProfileWriteRepository { get; }
        IUserReadRepository UserReadRepository { get; }
        IUserWriteRepository UserWriteRepository { get; }
        IProfilePictureReadRepository ProfilePictureReadRepository { get; }
    }
}
