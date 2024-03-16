using CoreService.Application.MQContracts.Commands.Abstracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.MQContracts.Commands.Concretes
{
    public struct CreateProfilePictureMQCommand : ICreateProfilePictureMQCommand
    {
        public CreateProfilePictureMQCommand()
        {
            UserId = string.Empty;
            UserProfileId = string.Empty;
            ProfilePictureData = new byte[0];
            ProfilePictureExtension = string.Empty;
            ProfilePictureName = string.Empty;
        }
        public string UserId { get; set; }
        public string UserProfileId { get; set; }
        public byte[] ProfilePictureData { get; set; }
        public string ProfilePictureExtension { get; set; }
        public string ProfilePictureName { get; set; }
    }
}
