// src/Application/Features/Subscriptions/Events/PaymentSucceededEvent.cs
using MediatR;
using SaaS.MaundCalculator.Domain.Entities.TenantAggregate;

namespace SaaS.MaundCalculator.Application.Features.Subscriptions.Events;

public record PaymentSucceededEvent(string SessionId) : INotification;

public class PaymentSucceededEventHandler : INotificationHandler<PaymentSucceededEvent>
{
    private readonly ParentDbContext _parent;
    private readonly ITenantService _tenantService;

    public PaymentSucceededEventHandler(ParentDbContext parent, ITenantService tenantService)
    {
        _parent = parent;
        _tenantService = tenantService;
    }

    public async Task Handle(PaymentSucceededEvent notification, CancellationToken ct)
    {
        var session = await new SessionService().GetAsync(notification.SessionId, cancellationToken: ct);
        var tenantId = Guid.Parse(session.Metadata["tenant_id"]);

        var tenant = await _parent.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId, ct)
            ?? throw new NotFoundException("Tenant not found.");

        var subscription = Subscription.Create(
            tenantId: tenant.Id,
            planId: session.Subscription!.Id,
            price: Money.From(session.AmountTotal / 100m)
        );

        tenant.ActivateSubscription(subscription);
        await _parent.SaveChangesAsync(ct);
    }
}