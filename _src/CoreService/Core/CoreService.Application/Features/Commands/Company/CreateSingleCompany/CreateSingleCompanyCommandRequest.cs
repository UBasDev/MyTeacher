using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.Company.CreateSingleCompany
{
    public class CreateSingleCompanyCommandRequest : IRequest<CreateSingleCompanyCommandResponse>
    {
        public CreateSingleCompanyCommandRequest()
        {
            Name = string.Empty;
            Description = null;
            Adress = null;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Adress { get; set; }
    }
}
