// src/Application/Features/Auth/Commands/LoginCommand.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using SaaS.MaundCalculator.Application.Common.Exceptions;
using SaaS.MaundCalculator.Application.Common.Interfaces;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthResult>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenService _jwtService;
    private readonly ITenantContext _tenantContext;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        IJwtTokenService jwtService,
        ITenantContext tenantContext)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _tenantContext = tenantContext;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        var valid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!valid) throw new UnauthorizedAccessException("Invalid credentials.");

        var token = await _jwtService.GenerateTokenAsync(user, _tenantContext.TenantId);
        return new AuthResult(token.AccessToken, token.RefreshToken);
    }
}