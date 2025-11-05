// src/Domain/Events/KapasRecordCreatedEvent.cs
namespace Domain.Events;

public record KapasRecordCreatedEvent(
    Guid RecordId,
    decimal WeightKg,
    Guid TenantId) : IDomainEvent;