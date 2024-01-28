using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.CreateSingleUser
{
    public class CreateSingleUserCommandRequest : IRequest<CreateSingleUserCommandResponse>
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }
        public UInt16 Age { get; set; } = 0;
    }
}
