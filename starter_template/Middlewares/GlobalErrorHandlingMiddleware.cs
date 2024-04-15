using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
namespace starter_template.Middlewares;

public class GlobalErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    public GlobalErrorHandlingMiddleware(ILogger logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            ProblemDetails error = new ProblemDetails()
            {
                Type = "",
                Title = "Something went wrong, please try again later",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = exception.Message,
                Instance = context.Request.Path
            };
            var jsonBody = JsonSerializer.Serialize(error);
            context.Response.ContentType = "application/json";
            _logger.Error(jsonBody);
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(jsonBody));

        }
    }
}