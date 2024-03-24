using MyTeacher.Helper.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Queries.Role.GetRolesExceptAdmin
{
    public class GetRolesWithoutAdminResponse : BaseResponse<List<GetRolesExceptAdminResponseModel>>
    {
    }

    public struct GetRolesExceptAdminResponseModel
    {
        public GetRolesExceptAdminResponseModel()
        {
            this.Name = string.Empty;
            this.ShortCode = string.Empty;
        }

        public string Name { get; set; }
        public string ShortCode { get; set; }
    }
}