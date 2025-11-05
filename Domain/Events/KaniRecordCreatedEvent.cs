// src/Domain/Events/KaniRecordCreatedEvent.cs
namespace Domain.Events;

public record KaniRecordCreatedEvent(Guid RecordId, decimal WeightKg) : IDomainEvent;