using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.Profile;
using CoreService.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Company
{
    public class CompanyEntity: BaseEntityWithSoftDelete<Guid>
    {
        public CompanyEntity()
        {
            Name = string.Empty;
            Description = null;
            Adress = null;
            Profiles = new List<ProfileEntity>();
        }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public string? Adress { get; private set; }
        public ICollection<ProfileEntity> Profiles { get; private set; }
    }
}
