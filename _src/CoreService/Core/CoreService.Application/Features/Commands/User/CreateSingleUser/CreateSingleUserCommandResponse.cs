using MyTeacher.Helper.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.CreateSingleUser
{
    public class CreateSingleUserCommandResponse : BaseResponse<CreateSingleUserCommandResponseModel>
    {
    }

    public class CreateSingleUserCommandResponseModel
    {
        public CreateSingleUserCommandResponseModel()
        {
            this.AccessToken = null;
            this.RefreshToken = null;
        }

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}