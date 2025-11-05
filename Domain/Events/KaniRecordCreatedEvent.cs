// src/Domain/Events/KaniRecordCreatedEvent.cs
namespace SaaS.MaundCalculator.Domain.Events;

public record KaniRecordCreatedEvent(Guid RecordId, decimal WeightKg) : IDomainEvent;