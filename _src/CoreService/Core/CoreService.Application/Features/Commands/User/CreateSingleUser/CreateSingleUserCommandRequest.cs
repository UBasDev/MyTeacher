using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace CoreService.Application.Features.Commands.User.CreateSingleUser
{
    public class CreateSingleUserCommandRequest : IRequest<CreateSingleUserCommandResponse>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UInt16 Age { get; set; } = 0;
        public IFormFile? ProfilePicture { get; set; }
        public static async Task<byte[]> StreamProfilePictureAndReturnAsByteArrayAsync(IFormFile profilePicture, CancellationToken cancellationToken) {
            using var fileStream = profilePicture.OpenReadStream();
            byte[] profilePictureBytes = new byte[profilePicture.Length];
            await fileStream.ReadAsync(profilePictureBytes.AsMemory(0, (int)profilePicture.Length), cancellationToken);
            return profilePictureBytes;
        }
    }
}
