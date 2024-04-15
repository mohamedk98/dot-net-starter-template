using System.Text;
using CorrelationId.DependencyInjection;
using CorrelationId.HttpClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using starter_template.Data;
using starter_template.Helpers;
using starter_template.Interfaces;
using starter_template.Middlewares;
using starter_template.Services;

namespace starter_template.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configurations)
    {
        services.AddTransient<GlobalErrorHandlingMiddleware>();
        services.AddHttpContextAccessor();
        services.AddDefaultCorrelationId(options =>
        {
            options.RequestHeader = "x-correlation-id";
            options.ResponseHeader = "x-correlation-id";
        });


        // Add services to the container.
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c => { c.DocumentFilter<PathPrefixInsertDocumentFilter>("/api"); });


        //Database Connection
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(configurations.GetConnectionString("DatabaseConnectionString"));
        });
        //Authentication Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();

        //Typed HTTP Client
        services.AddHttpClient<CustomHttpClient>((serviceProvider, httpClient) =>
        {
            httpClient.BaseAddress = new Uri("http://localhost:5000/api");
        }).AddCorrelationIdForwarding();


        //Jwt
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configurations.GetValue<string>("JwtKey"))),
                ValidateIssuer = false,
                ValidateAudience = false
            });


        return services;
    }
}