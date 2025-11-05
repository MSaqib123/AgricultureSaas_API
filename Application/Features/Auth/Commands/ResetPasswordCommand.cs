// src/Application/Features/Auth/Commands/ResetPasswordCommand.cs
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Features.Auth.Commands;

public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Unit>;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.NewPassword).MinimumLength(8);
    }
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
{
    private readonly UserManager<AppUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
        => _userManager = userManager;

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException("User not found.");

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(e => e.Description));

        return Unit.Value;
    }
}