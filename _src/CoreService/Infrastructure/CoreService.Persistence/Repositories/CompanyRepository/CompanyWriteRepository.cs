using CoreService.Application.Contexts;
using CoreService.Application.Repositories.CompanyRepository;
using CoreService.Domain.Entities.Company;
using CoreService.Persistence.Repositories.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Persistence.Repositories.CompanyRepository
{
    public class CompanyWriteRepository(ApplicationDbContext _dbContext): GenericWriteRepository<CompanyEntity>(_dbContext), ICompanyWriteRepository
    {
    }
}
