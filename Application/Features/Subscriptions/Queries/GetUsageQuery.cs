// src/Application/Features/Subscriptions/Queries/GetUsageQuery.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS.MaundCalculator.Application.Common.Interfaces;

namespace SaaS.MaundCalculator.Application.Features.Subscriptions.Queries;

public record GetUsageQuery : IRequest<UsageDto>;

public record UsageDto(int RecordsThisMonth, int MaxAllowed, bool WithinLimit);

public class GetUsageQueryHandler : IRequestHandler<GetUsageQuery, UsageDto>
{
    private readonly ITenantDbContextFactory _factory;
    private readonly ISubscriptionService _subscription;
    private readonly ITenantContext _tenant;

    public GetUsageQueryHandler(
        ITenantDbContextFactory factory,
        ISubscriptionService subscription,
        ITenantContext tenant)
    {
        _factory = factory;
        _subscription = subscription;
        _tenant = tenant;
    }

    public async Task<UsageDto> Handle(GetUsageQuery request, CancellationToken ct)
    {
        using var ctx = _factory.CreateDbContext();

        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var count = await ctx.KapasRecords
            .Where(r => r.TenantId == _tenant.TenantId && r.RecordedAt >= startOfMonth)
            .CountAsync(ct);

        var limit = await _subscription.GetMonthlyLimitAsync(_tenant.TenantId, ct);

        return new UsageDto(count, limit, count <= limit);
    }
}