// src/Domain/Events/SubscriptionActivatedEvent.cs
namespace Domain.Events;

public record SubscriptionActivatedEvent(Guid TenantId, Guid SubscriptionId) : IDomainEvent;