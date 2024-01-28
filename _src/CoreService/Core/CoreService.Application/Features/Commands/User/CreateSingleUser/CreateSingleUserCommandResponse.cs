using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.CreateSingleUser
{
    public class CreateSingleUserCommandResponse
    {
        public bool IsSuccessfull { get; set; } = true;
        public string? ErrorMessage { get; set; } = string.Empty;
    }
}
