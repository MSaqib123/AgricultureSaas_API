// src/Application/Features/Auth/Commands/Enable2FACommand.cs
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Features.Auth.Commands;

[Authorize]
public record Enable2FACommand : IRequest<TwoFactorResult>;

public record TwoFactorResult(string QrCodeUri, string ManualCode);

public class Enable2FACommandHandler : IRequestHandler<Enable2FACommand, TwoFactorResult>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _http;

    public Enable2FACommandHandler(UserManager<AppUser> userManager, IHttpContextAccessor http)
    {
        _userManager = userManager;
        _http = http;
    }

    public async Task<TwoFactorResult> Handle(Enable2FACommand request, CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(_http.HttpContext!.User)
            ?? throw new UnauthorizedAccessException();

        var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider);
        var qr = $"otpauth://totp/MaundCalc:{user.Email}?secret={code}&issuer=MaundCalc";

        return new TwoFactorResult(qr, code);
    }
}