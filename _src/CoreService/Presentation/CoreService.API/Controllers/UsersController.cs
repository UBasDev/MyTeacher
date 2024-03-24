using Microsoft.AspNetCore.Mvc;
using MediatR;
using CoreService.Application.Features.Commands.User.CreateSingleUser;
using CoreService.Application.Features.Commands.User.Login;
using MyTeacher.Helper.Attributes;
using MyTeacher.Helper.Models;
using CoreService.Application.Repositories;

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator, IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private UserModel? RequestUser
        {
            get
            {
                if (HttpContext.Items.TryGetValue("RequestUser", out var requestUser) && requestUser is UserModel userModel) return userModel;
                return null;
            }
        }

        private readonly IMediator _mediator = mediator;

        [HttpPost("create-single-user")]
        public async Task<CreateSingleUserCommandResponse> CreateSingleUser([FromForm] CreateSingleUserCommandRequest requestBody, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(requestBody, cancellationToken);
            if (!response.IsSuccessful) Response.StatusCode = 400;
            response.TraceId = HttpContext.TraceIdentifier;
            return response;
        }

        [HttpPost("login")]
        public async Task<LoginCommandResponse> Login([FromBody] LoginCommandRequest requestBody, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(requestBody, cancellationToken);
            if (!response.IsSuccessful) Response.StatusCode = 400;
            response.TraceId = HttpContext.TraceIdentifier;
            return response;
        }

        [GlobalAuthorize]
        [HttpGet("[action]")]
        public IActionResult Authorized1()
        {
            var x1 = RequestUser;
            return Ok("Tokenın kabul edildi");
        }

        [GlobalAuthorize("admin")]
        [HttpGet("[action]")]
        public IActionResult AuthorizedByAdmin1()
        {
            return Ok("Tokenın kabul edildi");
        }

        [GlobalAuthorize("CEO", "CTO")]
        [HttpGet("[action]")]
        public IActionResult AuthorizedByCLevel1()
        {
            return Ok("Tokenın kabul edildi");
        }

        /*
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
        */
    }
}