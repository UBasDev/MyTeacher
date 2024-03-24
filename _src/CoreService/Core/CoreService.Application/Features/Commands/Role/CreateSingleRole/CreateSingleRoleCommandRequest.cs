using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.Role.CreateSingleRole
{
    public class CreateSingleRoleCommandRequest : IRequest<CreateSingleRoleCommandResponse>
    {
        public CreateSingleRoleCommandRequest()
        {
            this.Name = string.Empty;
            this.ShortCode = string.Empty;
            this.Level = 0;
            this.Description = null;
        }

        public string Name { get; set; }
        public string ShortCode { get; set; }
        public UInt16 Level { get; set; }
        public string? Description { get; set; }
    }
}