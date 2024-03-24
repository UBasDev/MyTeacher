using CoreService.Application.Repositories;
using CoreService.Domain.Entities.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Features.Commands.Company.CreateSingleCompany
{
    public class CreateSingleCompanyCommandHandler(ILogger<CreateSingleCompanyCommandHandler> logger, IUnitOfWork unitOfWork) : IRequestHandler<CreateSingleCompanyCommandRequest, CreateSingleCompanyCommandResponse>
    {
        private readonly ILogger<CreateSingleCompanyCommandHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CreateSingleCompanyCommandResponse> Handle(CreateSingleCompanyCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateSingleCompanyCommandResponse();
            try
            {
                var isCompanyAlreadyExist = await _unitOfWork.CompanyReadRepository.FindByConditionAsNoTracking(c => c.Name.ToLower().Contains(request.Name.ToLower())).AnyAsync(cancellationToken);
                if (isCompanyAlreadyExist)
                {
                    response.ErrorMessage = "This company name already exists!";
                    response.IsSuccessful = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                await _unitOfWork.CompanyWriteRepository.InsertSingleAsync(CompanyEntity.CreateNewCompany(request.Name, request.Description, request.Adress), cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating new company. Request: {@Request} Error: {@Error}", request, ex);
                response.IsSuccessful = false;
                response.ErrorMessage = $"An unexpected error occurred while creating new company. Error: {ex.Message}";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}