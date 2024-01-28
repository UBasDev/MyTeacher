using CoreService.Application.Repositories;
using CoreService.Domain.Entities.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.Login
{
    public class LoginCommandHandler(IUnitOfWork unitOfWork, ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly ILogger<LoginCommandHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new LoginCommandResponse();
            var foundUser = await _unitOfWork.UserReadRepository.FindByCondition(u => u.Username == request.Username).FirstOrDefaultAsync();
            if (foundUser == null)
            {
                response.IsSuccessfull = false;
                response.ErrorMessage = "There is no user with given username";
                return response;
            }
            if (foundUser.PasswordHash != UserEntity.ComputeHash(request.Password, foundUser.PasswordSalt))
            {
                response.IsSuccessfull = false;
                response.ErrorMessage = "Your password is wrong";
                return response;
            }
            return response;
        }
    }
}
