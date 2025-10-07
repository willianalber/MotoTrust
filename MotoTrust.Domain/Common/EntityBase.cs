using MotoTrust.Domain.Events;
using MediatR;

namespace MotoTrust.Domain.Common;

public abstract class EntityBase
{
    private readonly List<DomainEvent> _domainEvents = new();

    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsActive { get; protected set; }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected EntityBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    protected EntityBase(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void Update()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
