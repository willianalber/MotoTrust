using MediatR;

namespace MotoTrust.Domain.Events;

public abstract class DomainEvent : INotification
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}
