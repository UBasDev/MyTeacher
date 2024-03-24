﻿using CoreService.Application.Repositories;
using CoreService.Domain.DomainEvents.User;
using CoreService.Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyTeacher.JWT.Abstracts;
using MyTeacher.JWT.TokenGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.User.CreateSingleUser
{
    internal class CreateSingleUserCommandHandler(ILogger<CreateSingleUserCommandHandler> logger, IUnitOfWork unitOfWork, IPublisher publisher, ITokenGenerator tokenGenerator) : IRequestHandler<CreateSingleUserCommandRequest, CreateSingleUserCommandResponse>
    {
        private readonly ILogger<CreateSingleUserCommandHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPublisher _publisher = publisher;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;

        public async Task<CreateSingleUserCommandResponse> Handle(CreateSingleUserCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateSingleUserCommandResponse();
            try
            {
                var isUserWithSameUsernameAlreadyExists = await _unitOfWork.UserReadRepository.FindByConditionAsNoTracking(u => u.Username == request.Username).AnyAsync(cancellationToken);
                if (isUserWithSameUsernameAlreadyExists)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "This username is already taken";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var isUserWithSameEmailAlreadyExists = await _unitOfWork.UserReadRepository.FindByConditionAsNoTracking(u => u.Email == request.Email).AnyAsync(cancellationToken);
                if (isUserWithSameEmailAlreadyExists)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "This email is already taken";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var userToCreate = UserEntity.CreateNewUser(request.Username, request.Email, request.Password);

                var foundRole = await _unitOfWork.RoleReadRepository.FindByCondition(r => r.ShortCode == request.RoleCode).FirstOrDefaultAsync();
                if (foundRole is null)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = "This role doesn't exist";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                userToCreate.SetRole(foundRole);

                await _unitOfWork.UserWriteRepository.InsertSingleAsync(userToCreate, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (request.ProfilePicture != null)
                {
                    if (request.ProfilePicture?.Length < 0)
                    {
                        response.IsSuccessful = false;
                        response.ErrorMessage = "This file is empty";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        return response;
                    }
                    var profilePictureBytes = await CreateSingleUserCommandRequest.StreamProfilePictureAndReturnAsByteArrayAsync(request.ProfilePicture, cancellationToken);

                    userToCreate.CreateProfileWithPictureWhenUserCreated(userToCreate.Id, request.Age, request.Firstname, request.Lastname, request.BirthDate, profilePictureBytes, Path.GetExtension(request.ProfilePicture.FileName), Path.GetFileNameWithoutExtension(request.ProfilePicture.FileName));
                }
                else
                {
                    userToCreate.CreateProfileWithoutPictureWhenUserCreated(userToCreate.Id, request.Age, request.Firstname, request.Lastname, request.BirthDate);
                }
                foreach (var currentEvent in userToCreate.DomainEvents)
                {
                    await _publisher.Publish(currentEvent, cancellationToken);
                }
                response.Payload.AccessToken = _tokenGenerator.GenerateJwtToken(userToCreate.Id.ToString(), userToCreate.Username, userToCreate.Email, foundRole.Name, TimeSpan.FromSeconds(300));
                response.Payload.RefreshToken = _tokenGenerator.GenerateJwtToken(userToCreate.Id.ToString(), userToCreate.Username, userToCreate.Email, foundRole.Name, TimeSpan.FromSeconds(600));
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while creating this user. Error: {@Error}", ex);
                response.IsSuccessful = false;
                response.ErrorMessage = $"An unexpected error occurred while creating this user. Error: {ex.Message}";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}