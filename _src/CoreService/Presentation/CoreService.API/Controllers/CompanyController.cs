using CoreService.Application.Features.Commands.Company.CreateSingleCompany;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("create-single-company")]
        public async Task<CreateSingleCompanyCommandResponse> CreateSingleCompany([FromBody] CreateSingleCompanyCommandRequest requestBody)
        {
            var response = await _mediator.Send(requestBody);
            response.TraceId = HttpContext.TraceIdentifier;
            return response;
        }
    }
}
