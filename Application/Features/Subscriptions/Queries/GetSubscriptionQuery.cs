// src/Application/Features/Subscriptions/Queries/GetSubscriptionQuery.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS.MaundCalculator.Application.Common.Interfaces;

namespace SaaS.MaundCalculator.Application.Features.Subscriptions.Queries;

public record GetSubscriptionQuery : IRequest<SubscriptionDto>;

public record SubscriptionDto(
    string PlanId,
    decimal Price,
    string Currency,
    DateTime StartsAt,
    DateTime? EndsAt,
    bool IsActive);

public class GetSubscriptionQueryHandler : IRequestHandler<GetSubscriptionQuery, SubscriptionDto>
{
    private readonly ParentDbContext _parent;
    private readonly ITenantContext _tenant;

    public GetSubscriptionQueryHandler(ParentDbContext parent, ITenantContext tenant)
    {
        _parent = parent;
        _tenant = tenant;
    }

    public async Task<SubscriptionDto> Handle(GetSubscriptionQuery request, CancellationToken ct)
    {
        var sub = await _parent.Subscriptions
            .AsNoTracking()
            .Where(s => s.TenantId == _tenant.TenantId && s.IsActive)
            .OrderByDescending(s => s.StartsAt)
            .FirstOrDefaultAsync(ct)
            ?? throw new NotFoundException("No active subscription.");

        return new SubscriptionDto(
            PlanId: sub.PlanId,
            Price: sub.Price.Amount,
            Currency: sub.Price.Currency,
            StartsAt: sub.StartsAt,
            EndsAt: sub.EndsAt,
            IsActive: sub.IsActive
        );
    }
}