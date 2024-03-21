using CoreService.Application.Repositories.GenericRepository;
using CoreService.Domain.Entities.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Application.Repositories.CompanyRepository
{
    public interface ICompanyReadRepository:IGenericReadRepository<CompanyEntity>
    {
    }
}
