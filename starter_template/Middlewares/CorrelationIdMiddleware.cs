using CorrelationId.Abstractions;
using Serilog.Context;
using ICorrelationIdProvider = starter_template.Interfaces.ICorrelationIdProvider;

namespace starter_template.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    private const string CorrelationIdHeader = "x-correlation-id";
    private readonly ICorrelationContextAccessor _correlationContext;

    public CorrelationIdMiddleware(RequestDelegate next, ICorrelationContextAccessor correlationContext)
    {
        _next = next;
        _correlationContext = correlationContext;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Get correlation id from header ot get ti from context
        var requestCorrelationId = GetCorrelationId(context);

        //Pushing the correlation id to the logger
        using (LogContext.PushProperty("CorrelationId", requestCorrelationId))
        {
            await _next(context);
        }
    }

    private string GetCorrelationId(HttpContext context)
    {
        //If correlation id exists in headers, return it back. otherwise, get the id from correlation context
        if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out
                var existingCorrelationId))
        {
            return existingCorrelationId.First();
        }

        var correlationIdFromContext = _correlationContext.CorrelationContext.CorrelationId;

        return correlationIdFromContext;
    }
}