using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.MQContracts.Commands.Abstracts
{
    public interface ICreateProfilePictureMQCommand
    {
        public string UserId { get; set; }
        public string UserProfileId { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
