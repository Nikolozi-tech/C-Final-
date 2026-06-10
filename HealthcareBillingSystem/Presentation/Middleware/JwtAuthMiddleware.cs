using Microsoft.AspNetCore.Http;

namespace HealthcareBillingSystem.Presentation.Middleware;

public class JwtAuthMiddleware
{
    private readonly RequestDelegate _next;

    public JwtAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authorizationHeader = context.Request.Headers.Authorization.ToString();
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Items["JwtToken"] = authorizationHeader["Bearer ".Length..].Trim();
        }

        await _next(context);
    }
}
