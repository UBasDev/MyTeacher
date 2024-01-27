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

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUnitOfWork unitOfWork, IRabbitMqPublisherService rabbitMqService, IPublisher publisher) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRabbitMqPublisherService _rabbitMqService = rabbitMqService;
        private readonly IPublisher _publisher = publisher;
        [HttpGet("[action]")]
        public async Task<IActionResult> CreateSingleUser(CancellationToken cancellationToken)
        {
            var userToCreate = UserEntity.CreateNewUser("ali1", "ali1@gmail.com", "salt1", "hash1");
            await _unitOfWork.UserWriteRepository.InsertSingleAsync(userToCreate);
            await _unitOfWork.SaveChangesAsync();
            await _publisher.Publish(new CreateNewProfileWhenUserCreatedDomainEvent(userToCreate.Id, 33), cancellationToken);
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
