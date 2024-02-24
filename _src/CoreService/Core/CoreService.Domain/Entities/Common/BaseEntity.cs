using System.ComponentModel.DataAnnotations;

namespace CoreService.Domain.Entities.Common
{
    public class BaseEntity<T> : AggregateRoot
    {
        protected BaseEntity()
        {

        }
        [Key]
        public T Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    }
}
