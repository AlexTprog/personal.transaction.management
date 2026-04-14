using MediatR;
using personal.transaction.management.domain.events;

namespace personal.transaction.management.application.Common;

public record DomainEventNotification<TEvent>(TEvent Event) : INotification where TEvent : IDomainEvent;
