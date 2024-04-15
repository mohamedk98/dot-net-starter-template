using CorrelationId;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using starter_template.Extensions;
using starter_template.Middlewares;
using CorrelationIdMiddleware = starter_template.Middlewares.CorrelationIdMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .CreateLogger();



builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
    configuration.Enrich.FromLogContext();
    configuration.WriteTo.Console(
        outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss} | {CorrelationId} | {Level:u3}] {Message:lj}{NewLine}",
        theme: AnsiConsoleTheme.Code
        
    );
});
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseCorrelationId();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //Both changes are required in swagger to allow the route prefix to be /api-docs
    app.UseSwagger(c => { c.RouteTemplate = "api-docs/{documentName}/swagger.json"; });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"v1/swagger.json", app.Environment.ApplicationName);
        c.RoutePrefix = $"api-docs";
    });
}



app.MapControllers();

app.UsePathBase(new PathString("/api"));
// app.UseHttpsRedirection();


app.Run();