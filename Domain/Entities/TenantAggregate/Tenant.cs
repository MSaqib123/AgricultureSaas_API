// src/Domain/Entities/TenantAggregate/Tenant.cs
using Domain.Events;
using SaaS.MaundCalculator.Domain.Entities;
using SaaS.MaundCalculator.Domain.Entities.TenantAggregate;
using SaaS.MaundCalculator.Domain.Events;

namespace Domain.Entities.TenantAggregate;

public class Tenant : AggregateRoot<Guid>, ITenantEntity
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Subdomain { get; private set; } = null!;
    public string? DatabaseName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Subscription? ActiveSubscription { get; private set; }

    private readonly List<AppUser> _users = new();
    public IReadOnlyCollection<AppUser> Users => _users.AsReadOnly();

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    private Tenant() { }

    public static Tenant Create(string name, string subdomain)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(subdomain)) throw new ArgumentException("Subdomain is required.", nameof(subdomain));

        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Subdomain = subdomain.Trim().ToLowerInvariant(),
            IsActive = false
        };

        tenant.AddDomainEvent(new TenantCreatedEvent(tenant.Id));
        return tenant;
    }

    public void ActivateSubscription(Subscription subscription)
    {
        ActiveSubscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
        IsActive = true;
        AddDomainEvent(new SubscriptionActivatedEvent(Id, subscription.Id));
    }

    public void Deactivate(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required.", nameof(reason));

        IsActive = false;
        AddDomainEvent(new TenantDeactivatedEvent(Id, reason.Trim()));
    }

    public void AssignDatabase(string dbName)
    {
        if (string.IsNullOrWhiteSpace(dbName))
            throw new ArgumentException("Database name is required.", nameof(dbName));

        if (string.IsNullOrEmpty(DatabaseName))
        {
            DatabaseName = dbName.Trim();
            AddDomainEvent(new TenantDatabaseAssignedEvent(Id, DatabaseName));
        }
    }

    protected void AddDomainEvent(IDomainEvent @event)
        => _domainEvents.Add(@event);
}