// src/Application/Common/Behaviors/PerformanceBehavior.cs
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SaaS.MaundCalculator.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly Stopwatch _timer;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsed = _timer.ElapsedMilliseconds;
        if (elapsed > 500)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} ms) {@Request}",
                requestName, elapsed, request);
        }

        return response;
    }
}