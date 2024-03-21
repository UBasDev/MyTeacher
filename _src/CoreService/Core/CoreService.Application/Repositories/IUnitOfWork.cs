using CoreService.Application.Repositories.CompanyRepository;
using CoreService.Application.Repositories.ProfilePicture;
using CoreService.Application.Repositories.ProfilePictureRepository;
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
        Task SaveChangesAsync(CancellationToken cancellationToken);
        IProfileReadRepository ProfileReadRepository { get; }
        IProfileWriteRepository ProfileWriteRepository { get; }
        IUserReadRepository UserReadRepository { get; }
        IUserWriteRepository UserWriteRepository { get; }
        IProfilePictureReadRepository ProfilePictureReadRepository { get; }
        IProfilePictureWriteRepository ProfilePictureWriteRepository { get; }
        ICompanyReadRepository CompanyReadRepository { get; }
        ICompanyWriteRepository CompanyWriteRepository { get; }
    }
}
