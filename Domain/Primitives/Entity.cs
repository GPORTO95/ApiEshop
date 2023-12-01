namespace Domain.Primitives;

public abstract class Entity<TEntityId> : IEntity
{
    private readonly List<DomainEvent> _domainEvents = new();

    protected Entity(TEntityId id)
    {
        Id = id;
    }

    protected Entity()
    { 
    }

    public TEntityId Id { get; init; }

    public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents;

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
