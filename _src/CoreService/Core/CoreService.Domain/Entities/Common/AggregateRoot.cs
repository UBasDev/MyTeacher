using CoreService.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreService.Domain.Entities.Common
{
    public abstract class AggregateRoot
    {
        protected AggregateRoot() { }
        [NotMapped]
        private readonly List<IDomainEvent> domainEvents = new List<IDomainEvent>();
        [NotMapped]
        public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();
        protected void AddDomainEvents(IDomainEvent notification) => domainEvents.Add(notification);
        public void ClearDomainEvents() => domainEvents.Clear();
    }
}
