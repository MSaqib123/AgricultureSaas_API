// src/Application/Features/Auth/Commands/UploadProfilePicCommand.cs
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SaaS.MaundCalculator.Application.Common.Interfaces;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Features.Auth.Commands;

[Authorize]
public record UploadProfilePicCommand(IFormFile File) : IRequest<string>;

public class UploadProfilePicCommandHandler : IRequestHandler<UploadProfilePicCommand, string>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _http;
    private readonly IFileStorageService _storage;

    public UploadProfilePicCommandHandler(
        UserManager<AppUser> userManager,
        IHttpContextAccessor http,
        IFileStorageService storage)
    {
        _userManager = userManager;
        _http = http;
        _storage = storage;
    }

    public async Task<string> Handle(UploadProfilePicCommand request, CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(_http.HttpContext!.User)
            ?? throw new UnauthorizedAccessException();

        var url = await _storage.UploadAsync(request.File, $"profile/{user.Id}");
        user.UpdateProfilePicture(url);

        await _userManager.UpdateAsync(user);
        return url;
    }
}