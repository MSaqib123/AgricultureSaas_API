// src/Application/Common/Behaviors/CachingBehavior.cs
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SaaS.MaundCalculator.Application.Common.Behaviors;

public class CachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, ICacheableRequest
{
    private readonly IDistributedCache _cache;

    public CachingBehavior(IDistributedCache cache) => _cache = cache;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var cacheKey = request.CacheKey;
        var cached = await _cache.GetStringAsync(cacheKey, ct);
        if (cached != null)
        {
            return JsonSerializer.Deserialize<TResponse>(cached)!;
        }

        var response = await next();
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(request.CacheDurationMinutes)
        };
        var serialized = JsonSerializer.Serialize(response);
        await _cache.SetStringAsync(cacheKey, serialized, cacheOptions, ct);

        return response;
    }
}

public interface ICacheableRequest
{
    string CacheKey { get; }
    int CacheDurationMinutes { get; }
}