using System.Diagnostics;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MU.CV.API.Apis;
// using MU.CV.API.ExceptionHandler;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MU.CV.API.Extensions;
using MU.CV.BLL.Extensions;
using MU.CV.DAL.Extensions;

namespace MU.CV.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddProblemProcessor();
        builder.AddBasicServiceDefaults();

        builder.Services.AddCVDataContext(builder.Configuration.GetConnectionString("CVDb"));
        builder.Services.AddCVRepositories();
        builder.Services.AddCVServices();
        // Add services to the container.
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                //TODO: все значения в appsettings
                const string realm = "resume-realm";
                const string issuer = "http://localhost:9090/realms/" + realm;
                options.Authority = issuer;
                options.RequireHttpsMetadata = false;         

                // options.Audience  = "resume-api";            
                //options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    
                    ValidateAudience = true,
                    ValidAudience = "resume-api",
                    
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    
                    // mappings
                    NameClaimType = "preferred_username",
                    RoleClaimType = "roles"
                };

                // logs for 401
                // TODO: only for development
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        ctx.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JWT")
                            .LogError(ctx.Exception, "Auth failed");
                        return Task.CompletedTask;
                    }
                };
            });                                               

        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Resume API", Version = "v1" });

            const string realm = "resume-realm";
            //TODO: все значения в appsettings
            var issuer = $"http://localhost:9090/realms/{realm}";

            // Схема безопасности OAuth2 (Authorization Code + PKCE)
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{issuer}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{issuer}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID scope" }
                        }
                    }
                }
            });

            // Требовать авторизацию по умолчанию для всех операций (можно тоньше через OperationFilter)
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new[] { "openid" } // запрашиваемые скоупы
                }
            });
        });
        
        builder.Services
            .AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.ApiVersionReader = new HeaderApiVersionReader("Api-Version");
            })
            // для Swagger с группировкой по версиям
            .AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

        var app = builder.Build();

        app.AddProblemDetails();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Resume API v1");

                // Настройки OAuth для кнопки "Authorize"
                //TODO: все значения в appsettings
                c.OAuthClientId("resume-frontend");
                // c.OAuthClientSecret("...");       // if client is confidential
                c.OAuthUsePkce();                    
                c.OAuthScopes("openid");
                // c.OAuthAdditionalQueryStringParams(new Dictionary<string,string>{{"ui_locales","ru"}});
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapDefaultEndpoints();
        
        var orders = app.NewVersionedApi();
        orders.MapNotesApiV1();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)]
                        })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

        app.Run();
    }
}
