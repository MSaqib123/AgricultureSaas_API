// src/Application/Features/Tenants/Queries/GetTenantQuery.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaaS.MaundCalculator.Application.Common.Interfaces;

namespace SaaS.MaundCalculator.Application.Features.Tenants.Queries;

public record GetTenantQuery(Guid TenantId) : IRequest<TenantDto>;

public record TenantDto(Guid Id, string Name, string Subdomain, bool IsActive, DateTime CreatedAt);

public class GetTenantQueryHandler : IRequestHandler<GetTenantQuery, TenantDto>
{
    private readonly ParentDbContext _parent;

    public GetTenantQueryHandler(ParentDbContext parent) => _parent = parent;

    public async Task<TenantDto> Handle(GetTenantQuery request, CancellationToken ct)
    {
        var tenant = await _parent.Tenants
            .AsNoTracking()
            .Where(t => t.Id == request.TenantId)
            .Select(t => new TenantDto(
                Id: t.Id,
                Name: t.Name,
                Subdomain: t.Subdomain,
                IsActive: t.IsActive,
                CreatedAt: t.CreatedAt
            ))
            .FirstOrDefaultAsync(ct)
            ?? throw new NotFoundException("Tenant not found.");

        return tenant;
    }
}