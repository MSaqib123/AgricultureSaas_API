// src/Application/Features/Auth/Commands/ConfirmEmailCommand.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Features.Auth.Commands;

public record ConfirmEmailCommand(string Email, string Token) : IRequest<Unit>;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
{
    private readonly UserManager<AppUser> _userManager;

    public ConfirmEmailCommandHandler(UserManager<AppUser> userManager)
        => _userManager = userManager;

    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description));

        return Unit.Value;
    }
}