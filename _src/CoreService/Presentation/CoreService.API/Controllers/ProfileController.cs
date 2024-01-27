using CoreService.Application.Repositories;
using CoreService.Domain.Entities.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult Test1()
        {
            return Ok();
        }
    }
}
