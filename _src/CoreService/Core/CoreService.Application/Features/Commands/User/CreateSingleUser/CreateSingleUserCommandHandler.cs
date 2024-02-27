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

            if (request.ProfilePicture != null)
            {
                if(request.ProfilePicture?.Length < 0)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "This file is empty";
                    return response;
                }
                using var fileStream = request.ProfilePicture.OpenReadStream();
                byte[] profilePictureBytes = new byte[request.ProfilePicture.Length];
                await fileStream.ReadAsync(profilePictureBytes.AsMemory(0, (int)request.ProfilePicture.Length), cancellationToken);

                userToCreate.CreateProfileWhenUserCreated(userToCreate.Id, request.Age, profilePictureBytes); 
            }
            response.SuccessMessage = "You've been successfully registered";
            
            foreach(var currentEvent in userToCreate.DomainEvents)
            {
                await _publisher.Publish(currentEvent, cancellationToken);
            }

            return response;
        }

    }
}
