
using System.ComponentModel.DataAnnotations;

namespace CoreService.Domain.Entities.Common
{
    public class BaseEntity
    {
        private BaseEntity()
        {

        }
        [Key]
        public Guid Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.Now;
        public DateTimeOffset? UpdatedAt { get; protected set; }
        public DateTimeOffset? DeletedAt { get; protected set; }
        public bool IsActive { get; protected set; } = true;
        public bool IsDeleted { get; protected set; } = false;

    }
}
