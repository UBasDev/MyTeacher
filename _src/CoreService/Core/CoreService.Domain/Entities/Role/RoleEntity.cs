using CoreService.Domain.Entities.Common;
using CoreService.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Role
{
    public sealed class RoleEntity : BaseEntityWithSoftDelete<Guid>
    {
        public RoleEntity()
        {
            Name = string.Empty;
            ShortCode = string.Empty;
            Level = 0;
            Description = null;
            Users = new List<UserEntity>();
        }

        private RoleEntity(string name, string shortCode, UInt16 level, string? description)
        {
            Name = name;
            ShortCode = shortCode;
            Level = level;
            Description = description;
        }

        public string Name { get; private set; }
        public string ShortCode { get; private set; }

        [Range(1, 5000, ErrorMessage = "Role level should be between 1 and 5000")]
        public UInt16 Level { get; private set; }

        public string? Description { get; private set; }
        public ICollection<UserEntity> Users { get; private set; }

        public static RoleEntity CreateNewRoleEntity(string name, string shortCode, UInt16 level, string? description)
        {
            return new RoleEntity(name, shortCode, level, description);
        }
    }
}