// src/Domain/Events/TenantDeactivatedEvent.cs
namespace Domain.Events;

public record TenantDeactivatedEvent(Guid TenantId, string Reason) : IDomainEvent;