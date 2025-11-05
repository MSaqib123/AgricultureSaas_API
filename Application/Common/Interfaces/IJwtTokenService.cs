// src/Application/Common/Interfaces/IJwtTokenService.cs
using SaaS.MaundCalculator.Domain.Entities;

namespace SaaS.MaundCalculator.Application.Common.Interfaces;

public record TokenResult(string AccessToken, string RefreshToken);

public interface IJwtTokenService
{
    Task<TokenResult> GenerateTokenAsync(AppUser user, Guid? tenantId = null);
    Task<string> ValidateRefreshTokenAsync(string refreshToken);
}