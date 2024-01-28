using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Newtonsoft.Json;
using RabbitMQ.Abstracts;
using MediatR;
using CoreService.Application.Features.Commands.User.CreateSingleUser;
using CoreService.Application.Features.Commands.User.Login;

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator, IRabbitMqPublisherService rabbitMqService) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IRabbitMqPublisherService _rabbitMqService = rabbitMqService;

        [HttpPost("[action]")]
        public async Task<CreateSingleUserCommandResponse> CreateSingleUser([FromBody] CreateSingleUserCommandRequest requestBody, CancellationToken cancellationToken)
        {
            return await _mediator.Send(requestBody, cancellationToken);
        }

        [HttpPost("[action]")]
        public async Task<LoginCommandResponse> Login([FromBody] LoginCommandRequest requestBody, CancellationToken cancellationToken)
        {
            return await _mediator.Send(requestBody, cancellationToken);
        }

        [HttpGet("[action]")]
        public IActionResult PublishSingleMessageSync()
        {
            _rabbitMqService.PublishMessageSync("Merhabalar, bu mesajı sync olarak queueye gönderiyorum.");
            return Ok();
        }
        [HttpGet("PublishSingleMessageAsync")]
        public async Task<IActionResult> PublishSingleMessageAsync()
        {
            await _rabbitMqService.PublishMessageAsync("Merhabalar, bu mesajı async olarak queueye gönderiyorum.");
            return Ok();
        }
    }
}
