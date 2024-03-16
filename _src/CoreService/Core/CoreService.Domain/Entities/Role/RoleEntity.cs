using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Role
{
    sealed public class RoleEntity : BaseEntityWithSoftDelete<Guid>
    {
        public RoleEntity()
        {
            Name = string.Empty;
            ShortCode = string.Empty;
            Level = 0;
            Description = null;
            Users = new List<UserEntity>();
        }
        private RoleEntity(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public string ShortCode { get; private set; }
        public UInt16 Level { get; set; }
        public string? Description { get; private set; }
        public ICollection<UserEntity> Users { get; private set; }
    }
}
