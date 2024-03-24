using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace CoreService.Application.Features.Commands.User.CreateSingleUser
{
    public class CreateSingleUserCommandRequest : IRequest<CreateSingleUserCommandResponse>
    {
        public CreateSingleUserCommandRequest()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Age = 0;
            Firstname = string.Empty;
            Lastname = string.Empty;
            RoleCode = string.Empty;
            BirthDate = 0;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UInt16 Age { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RoleCode { get; set; }
        public ulong BirthDate { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        public static async Task<byte[]> StreamProfilePictureAndReturnAsByteArrayAsync(IFormFile profilePicture, CancellationToken cancellationToken)
        {
            using var fileStream = profilePicture.OpenReadStream();
            byte[] profilePictureBytes = new byte[profilePicture.Length];
            await fileStream.ReadAsync(profilePictureBytes.AsMemory(0, (int)profilePicture.Length), cancellationToken);
            return profilePictureBytes;
        }
    }
}