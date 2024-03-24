using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Queries.Role.GetRolesExceptAdmin
{
    public class GetRolesWithoutAdminRequest : IRequest<GetRolesWithoutAdminResponse>
    {
    }
}