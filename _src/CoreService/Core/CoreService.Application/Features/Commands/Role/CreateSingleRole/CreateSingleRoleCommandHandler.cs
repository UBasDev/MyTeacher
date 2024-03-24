using CoreService.Application.Repositories;
using CoreService.Domain.Entities.Role;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.Role.CreateSingleRole
{
    public class CreateSingleRoleCommandHandler(ILogger<CreateSingleRoleCommandHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<CreateSingleRoleCommandRequest, CreateSingleRoleCommandResponse>
    {
        private readonly ILogger<CreateSingleRoleCommandHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CreateSingleRoleCommandResponse> Handle(CreateSingleRoleCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateSingleRoleCommandResponse();
            try
            {
                var isRoleNameAlreadyExist = await _unitOfWork.RoleReadRepository.FindByConditionAsNoTracking(r => r.Name == request.Name).AnyAsync();
                if (isRoleNameAlreadyExist)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = $"This role name already exists";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var isRoleShortCodeAlreadyExist = await _unitOfWork.RoleReadRepository.FindByConditionAsNoTracking(r => r.ShortCode == request.ShortCode).AnyAsync();
                if (isRoleShortCodeAlreadyExist)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = $"This role short code already exists";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var isRoleLevelAlreadyExist = await _unitOfWork.RoleReadRepository.FindByConditionAsNoTracking(r => r.Level == request.Level).AnyAsync();
                if (isRoleLevelAlreadyExist)
                {
                    response.IsSuccessful = false;
                    response.ErrorMessage = $"This role level already exists";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                await _unitOfWork.RoleWriteRepository.InsertSingleAsync(RoleEntity.CreateNewRoleEntity(request.Name, request.ShortCode, request.Level, request.Description), cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while creating this role. Error: {@Error}", ex);
                response.IsSuccessful = false;
                response.ErrorMessage = $"An unexpected error occurred while creating this role. Error: {ex.Message}";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}