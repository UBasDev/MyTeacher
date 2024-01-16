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

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUnitOfWork unitOfWork, IRabbitMqPublisherService rabbitMqService) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRabbitMqPublisherService _rabbitMqService = rabbitMqService;
        [HttpGet("[action]")]
        public IActionResult CreateSingleUser()
        {
            _unitOfWork.UserWriteRepository.InsertSingle(UserEntity.CreateNewUser("ali1", "ali1@gmail.com", "salt1", "hash1", ProfileEntity.CreateNewProfile(31)));
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
