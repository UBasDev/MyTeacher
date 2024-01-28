using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CoreService.Application.Repositories;
using CoreService.Domain.Entities.User;
using CoreService.Domain.Entities.Profile;
using Newtonsoft.Json;
using RabbitMQ.Abstracts;
using MediatR;
using CoreService.Domain.DomainEvents.User;
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
        public async Task<IActionResult> CreateSingleUser([FromBody] CreateSingleUserCommandRequest requestBody, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(requestBody);
            if (!response.IsSuccessfull)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginCommandRequest requestBody, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(requestBody);
            if (!response.IsSuccessfull)
            {
                return BadRequest(response.ErrorMessage);
            }
            return Ok();
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
