// src/Application/Common/Behaviors/AuthorizationBehavior.cs
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SaaS.MaundCalculator.Application.Common.Exceptions;

namespace SaaS.MaundCalculator.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var authorizeAttributes = request.GetType()
            .GetCustomAttributes(typeof(AuthorizeAttribute), true);

        if (authorizeAttributes.Any())
        {
            var httpContext = _httpContextAccessor.HttpContext
                              ?? throw new UnauthorizedAccessException();

            if (!httpContext.User.Identity?.IsAuthenticated ?? false)
                throw new UnauthorizedAccessException();

            var authorize = (AuthorizeAttribute)authorizeAttributes[0];
            var policies = authorize.Policy?.Split(',') ?? Array.Empty<string>();
            var roles = authorize.Roles?.Split(',') ?? Array.Empty<string>();

            foreach (var policy in policies)
                if (!await httpContext.User.IsInRoleAsync(policy.Trim()))
                    throw new ForbiddenAccessException();

            foreach (var role in roles)
                if (!httpContext.User.IsInRole(role.Trim()))
                    throw new ForbiddenAccessException();
        }

        return await next();
    }
}