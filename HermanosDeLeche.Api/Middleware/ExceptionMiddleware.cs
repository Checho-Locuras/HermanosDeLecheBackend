using System.Net;
using System.Text.Json;
using HermanosDeLeche.Domain.Exceptions;

namespace HermanosDeLeche.Api.Middleware;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            _logger.LogWarning(ex, "Business rule violation");
            await WriteJsonAsync(context, ex.StatusCode, new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteJsonAsync(context, (int)HttpStatusCode.InternalServerError, new { error = "Error interno del servidor." });
        }
    }

    private static async Task WriteJsonAsync(HttpContext context, int statusCode, object payload)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
