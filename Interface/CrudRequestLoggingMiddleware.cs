using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Interface;

/// <summary>
/// Логирует HTTP-запросы CRUD (GET, POST, PUT, PATCH, DELETE) в Serilog — консоль и БД по настройкам.
/// </summary>
public sealed class CrudRequestLoggingMiddleware
{
    private static readonly HashSet<string> CrudMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        "GET", "POST", "PUT", "PATCH", "DELETE"
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<CrudRequestLoggingMiddleware> _logger;

    public CrudRequestLoggingMiddleware(RequestDelegate next, ILogger<CrudRequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!ShouldLog(context))
        {
            await _next(context);
            return;
        }

        var sw = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            var user = context.User?.Identity?.IsAuthenticated == true
                ? context.User.Identity?.Name
                : null;

            _logger.LogInformation(
                "CRUD {HttpMethod} {RequestPath} status {StatusCode} in {ElapsedMs} ms {UserName}",
                context.Request.Method,
                $"{context.Request.Path}{context.Request.QueryString}",
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                user ?? "(anonymous)");
        }
    }

    private static bool ShouldLog(HttpContext context)
    {
        if (!CrudMethods.Contains(context.Request.Method))
            return false;

        var path = context.Request.Path.Value ?? "";
        if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}

public static class CrudRequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseCrudRequestLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CrudRequestLoggingMiddleware>();
    }
}
