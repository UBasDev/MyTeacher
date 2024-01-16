
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CoreService.Domain.Entities.Common
{
    public class BaseEntity<T>
    {
        protected BaseEntity()
        {

        }
        [Key]
        public T Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; protected set; }
        public DateTimeOffset? DeletedAt { get; protected set; }
        public bool IsActive { get; protected set; } = true;
        public bool IsDeleted { get; protected set; } = false;

        private ICollection<INotification> domainEvents;
        //public ICollection<INotification> DomainEvents => domainEvents;

        protected void AddDomainEvents(INotification notification)
        {
            domainEvents ??= new List<INotification>();

            domainEvents.Add(notification);
        }

    }
}
