// src/Domain/Events/KapasRecordDeletedEvent.cs
namespace Domain.Events;

public record KapasRecordDeletedEvent(Guid RecordId, Guid TenantId) : IDomainEvent;