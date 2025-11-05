// src/Domain/Entities/IAggregateRoot.cs
using SaaS.MaundCalculator.Domain.Events;

namespace SaaS.MaundCalculator.Domain.Entities;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}