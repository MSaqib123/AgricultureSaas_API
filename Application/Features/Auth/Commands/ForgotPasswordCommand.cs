// src/Application/Features/Auth/Commands/ForgotPasswordCommand.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using SaaS.MaundCalculator.Application.Common.Interfaces;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Features.Auth.Commands;

public record ForgotPasswordCommand(string Email) : IRequest<Unit>;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _config;

    public ForgotPasswordCommandHandler(
        UserManager<AppUser> userManager,
        IEmailService emailService,
        IConfiguration config)
    {
        _userManager = userManager;
        _emailService = emailService;
        _config = config;
    }

    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return Unit.Value;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var domain = _config["App:Domain"] ?? "http://localhost:5000";
        var resetLink = $"{domain}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email!)}";

        await _emailService.SendEmailAsync(
            user.Email!,
            "Reset Your MaundCalc Password",
            $"Click <a href='{resetLink}'>here</a> to reset your password. Link expires in 1 hour.");

        return Unit.Value;
    }
}