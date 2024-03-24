using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Common
{
    public class BaseEntityWithSoftDelete<T> : AggregateRoot, ISoftDelete
    {
        public BaseEntityWithSoftDelete()
        {
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = null;
            DeletedAt = null;
            IsActive = true;
            IsDeleted = false;
        }

        [Key]
        public T Id { get; protected set; }

        public DateTimeOffset CreatedAt { get; protected set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}