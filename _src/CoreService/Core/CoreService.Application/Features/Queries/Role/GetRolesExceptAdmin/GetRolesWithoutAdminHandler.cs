using CoreService.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Queries.Role.GetRolesExceptAdmin
{
    public class GetRolesWithoutAdminHandler(ILogger<GetRolesWithoutAdminHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<GetRolesWithoutAdminRequest, GetRolesWithoutAdminResponse>
    {
        private readonly ILogger<GetRolesWithoutAdminHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<GetRolesWithoutAdminResponse> Handle(GetRolesWithoutAdminRequest request, CancellationToken cancellationToken)
        {
            var response = new GetRolesWithoutAdminResponse();
            try
            {
                var rolesWithoutAdmin = await _unitOfWork.RoleReadRepository.FindByConditionAsNoTracking(r => r.Name != "Admin").Select(r => new GetRolesExceptAdminResponseModel()
                {
                    Name = r.Name,
                    ShortCode = r.ShortCode,
                }).ToListAsync(cancellationToken);
                response.Payload = rolesWithoutAdmin;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred while receiving roles without admin. Error: {@Error}", ex);
                response.IsSuccessful = false;
                response.ErrorMessage = $"An unexpected error occurred while receiving roles without admin. Error: {ex.Message}";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}