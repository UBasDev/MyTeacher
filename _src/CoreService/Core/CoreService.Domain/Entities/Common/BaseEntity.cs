using System.ComponentModel.DataAnnotations;

namespace CoreService.Domain.Entities.Common
{
    public class BaseEntity<T> : AggregateRoot
    {
        protected BaseEntity()
        {
            CreatedAt = DateTimeOffset.UtcNow;
        }
        [Key]
        public T Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; }
    }
}
