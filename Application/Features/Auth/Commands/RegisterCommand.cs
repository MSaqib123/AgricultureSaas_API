// src/Application/Features/Auth/Commands/RegisterCommand.cs
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SaaS.MaundCalculator.Application.Common.Interfaces;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Features.Auth.Commands;

public record RegisterCommand(
    string Email,
    string Password,
    string FullName,
    string? Subdomain = null
) : IRequest<AuthResult>;

public record AuthResult(string AccessToken, string RefreshToken);

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        When(x => x.Subdomain != null, () =>
        {
            RuleFor(x => x.Subdomain).Matches(@"^[a-z0-9-]+$").WithMessage("Subdomain must be lowercase, numbers, hyphens only.");
        });
    }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITenantService _tenantService;
    private readonly IJwtTokenService _jwtService;

    public RegisterCommandHandler(
        UserManager<AppUser> userManager,
        ITenantService tenantService,
        IJwtTokenService jwtService)
    {
        _userManager = userManager;
        _tenantService = tenantService;
        _jwtService = jwtService;
    }

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken ct)
    {
        var user = AppUser.Create(request.Email, request.FullName);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description));

        Tenant? tenant = null;
        if (!string.IsNullOrWhiteSpace(request.Subdomain))
        {
            tenant = Tenant.Create($"{request.FullName}'s Farm", request.Subdomain);
            await _tenantService.CreateTenantAsync(tenant, user, ct);
        }

        var token = await _jwtService.GenerateTokenAsync(user, tenant?.Id);
        return new AuthResult(token.AccessToken, token.RefreshToken);
    }
}