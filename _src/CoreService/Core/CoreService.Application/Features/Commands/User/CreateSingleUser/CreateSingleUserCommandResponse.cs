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

    public struct CreateSingleUserCommandResponseModel
    {
        public CreateSingleUserCommandResponseModel()
        {
            Username = string.Empty;
            Email = string.Empty;
            RoleName = string.Empty;
            Firstname = string.Empty;
            Lastname = string.Empty;
            Age = 0;
            BirthDate = 0;
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string? RoleName { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public ushort? Age { get; set; }
        public ulong BirthDate { get; set; }
    }
}