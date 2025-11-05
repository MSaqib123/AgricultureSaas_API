// src/Application/Features/Tenants/Commands/CreateTenantCommand.cs
using MediatR;
using SaaS.MaundCalculator.Application.Common.Interfaces;
using SaaS.MaundCalculator.Domain.Entities;

namespace Application.Features.Tenants.Commands;

[Authorize(Roles = "Admin")]
public record CreateTenantCommand(string Name, string Subdomain) : IRequest<Guid>;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Guid>
{
    private readonly ITenantService _tenantService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _http;

    public CreateTenantCommandHandler(
        ITenantService tenantService,
        UserManager<AppUser> userManager,
        IHttpContextAccessor http)
    {
        _tenantService = tenantService;
        _userManager = userManager;
        _http = http;
    }

    public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken ct)
    {
        var currentUser = await _userManager.GetUserAsync(_http.HttpContext!.User)
            ?? throw new UnauthorizedAccessException();

        var tenant = Tenant.Create(request.Name, request.Subdomain);
        await _tenantService.CreateTenantAsync(tenant, currentUser, ct);

        return tenant.Id;
    }
}