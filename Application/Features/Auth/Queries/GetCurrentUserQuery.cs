// src/Application/Features/Auth/Queries/GetCurrentUserQuery.cs
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SaaS.MaundCalculator.Application.Common.Interfaces;
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Features.Auth.Queries;

[Authorize]
public record GetCurrentUserQuery : IRequest<UserDto>;

public record UserDto(Guid Id, string Email, string FullName, Guid? TenantId, string? ProfilePicUrl);

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _http;
    private readonly ITenantContext _tenant;

    public GetCurrentUserQueryHandler(
        UserManager<AppUser> userManager,
        IHttpContextAccessor http,
        ITenantContext tenant)
    {
        _userManager = userManager;
        _http = http;
        _tenant = tenant;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        var user = await _userManager.GetUserAsync(_http.HttpContext!.User)
            ?? throw new UnauthorizedAccessException();

        return new UserDto(
            Id: user.Id,
            Email: user.Email!,
            FullName: user.FullName,
            TenantId: _tenant.TenantId,
            ProfilePicUrl: user.ProfilePictureUrl
        );
    }
}