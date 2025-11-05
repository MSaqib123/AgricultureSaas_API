// src/Application/Features/Tenants/Queries/ListTenantsQuery.cs
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace SaaS.MaundCalculator.Application.Features.Tenants.Queries;

[Authorize(Roles = "SuperAdmin")]
public record ListTenantsQuery : IRequest<List<TenantSummaryDto>>;

public record TenantSummaryDto(Guid Id, string Name, string Subdomain, int UserCount, bool IsActive);

public class ListTenantsQueryHandler : IRequestHandler<ListTenantsQuery, List<TenantSummaryDto>>
{
    private readonly ParentDbContext _parent;

    public ListTenantsQueryHandler(ParentDbContext parent) => _parent = parent;

    public async Task<List<TenantSummaryDto>> Handle(ListTenantsQuery request, CancellationToken ct)
    {
        return await _parent.Tenants
            .AsNoTracking()
            .Select(t => new TenantSummaryDto(
                Id: t.Id,
                Name: t.Name,
                Subdomain: t.Subdomain,
                UserCount: t.Users.Count,
                IsActive: t.IsActive
            ))
            .OrderBy(t => t.Name)
            .ToListAsync(ct);
    }
}