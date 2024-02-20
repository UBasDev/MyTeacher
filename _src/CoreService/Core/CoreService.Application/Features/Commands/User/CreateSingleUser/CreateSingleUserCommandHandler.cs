using CoreService.Application.Repositories;
using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.CreateSingleUser
{
    internal class CreateSingleUserCommandHandler(IUnitOfWork unitOfWork, IPublisher publisher) : IRequestHandler<CreateSingleUserCommandRequest, CreateSingleUserCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPublisher _publisher = publisher;
        public async Task<CreateSingleUserCommandResponse> Handle(CreateSingleUserCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateSingleUserCommandResponse();
            var isUserWithSameUsernameAlreadyExists = await _unitOfWork.UserReadRepository.FindByCondition(u => u.Username == request.Username).AsNoTracking().AnyAsync(cancellationToken);
            if (isUserWithSameUsernameAlreadyExists)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "This username is already taken";
                return response;
            }
            var isUserWithSameEmailAlreadyExists = await _unitOfWork.UserReadRepository.FindByCondition(u => u.Email == request.Email).AsNoTracking().AnyAsync(cancellationToken);
            if (isUserWithSameEmailAlreadyExists)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "This email is already taken";
                return response;
            }
            var userToCreate = UserEntity.CreateNewUser(request.Username, request.Email, request.Password);

            await _unitOfWork.UserWriteRepository.InsertSingleAsync(userToCreate);
            await _unitOfWork.SaveChangesAsync();
            userToCreate.AddProfileWhenUserCreated(new CreateNewProfileWhenUserCreatedDomainEvent(userToCreate.Id, request.Age));
            response.SuccessMessage = "You've been successfully registered";
            await _publisher.Publish(userToCreate.DomainEvents, cancellationToken);
            return response;
        }

    }
}
