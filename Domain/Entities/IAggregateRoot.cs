// src/Domain/Entities/IAggregateRoot.cs
using Domain.Events;

namespace Domain.Entities;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}