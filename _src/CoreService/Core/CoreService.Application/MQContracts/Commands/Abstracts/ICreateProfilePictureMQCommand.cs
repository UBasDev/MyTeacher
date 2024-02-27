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
        public Guid UserId { get; set; }
        public Guid UserProfileId { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
