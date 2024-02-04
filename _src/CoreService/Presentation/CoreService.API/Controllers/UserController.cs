﻿using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Abstracts;
using MediatR;
using CoreService.Application.Features.Commands.User.CreateSingleUser;
using CoreService.Application.Features.Commands.User.Login;
using MyTeacher.Helper.Attributes;
using MyTeacher.Helper.Models;

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator, IRabbitMqPublisherService rabbitMqService, UserModel userModel) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IRabbitMqPublisherService _rabbitMqService = rabbitMqService;
        private readonly UserModel _userModel = userModel;
        [HttpPost("[action]")]
        public async Task<CreateSingleUserCommandResponse> CreateSingleUser([FromBody] CreateSingleUserCommandRequest requestBody, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(requestBody, cancellationToken);
            if (!response.IsSuccessful) Response.StatusCode = 400;
            response.TraceId = HttpContext.TraceIdentifier;
            return response;
        }
        [HttpPost("[action]")]
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
            var x1 = _userModel;
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
