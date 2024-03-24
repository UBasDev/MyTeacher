using CoreService.Application.Features.Commands.Role.CreateSingleRole;
using CoreService.Application.Features.Queries.Role.GetRolesExceptAdmin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("create-single-role")]
        public async Task<CreateSingleRoleCommandResponse> CreateSingleRole([FromBody] CreateSingleRoleCommandRequest requestBody)
        {
            var response = await _mediator.Send(requestBody);
            response.TraceId = HttpContext.TraceIdentifier;
            return response;
        }

        [HttpGet("get-roles-without-admin")]
        public async Task<GetRolesWithoutAdminResponse> GetRolesWithoutAdminResponse()
        {
            var response = await _mediator.Send(new GetRolesWithoutAdminRequest());
            response.TraceId = HttpContext.TraceIdentifier;
            return response;
        }
    }
}