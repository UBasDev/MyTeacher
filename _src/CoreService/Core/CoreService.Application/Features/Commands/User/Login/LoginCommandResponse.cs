using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.Login
{
    public class LoginCommandResponse
    {
        public bool IsSuccessfull { get; set; } = true;
        public string? ErrorMessage { get; set; } = string.Empty;
    }
}
