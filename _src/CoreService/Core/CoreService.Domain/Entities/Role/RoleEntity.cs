using CoreService.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Role
{
    sealed public class RoleEntity : BaseEntity<Guid>, ISoftDelete
    {
        private RoleEntity(string name)
        {
            Name = name;
        }
        public string Name { get; private set; } = string.Empty;

        public DateTimeOffset? UpdatedAt { get; private set; }
        public DateTimeOffset? DeletedAt { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
    }
}
