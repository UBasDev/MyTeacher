using MyTeacher.Helper.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.Login
{
    public class LoginCommandResponse : BaseResponse<LoginCommandResponseModel>
    {
    }
    public class LoginCommandResponseModel
    {
        public LoginCommandResponseModel()
        {
            AccessToken = null;
            RefreshToken = null;
        }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
