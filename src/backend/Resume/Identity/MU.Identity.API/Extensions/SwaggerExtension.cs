using Microsoft.OpenApi.Models;

namespace MU.Identity.API.Extensions;

public static class SwaggerExtension
{
    public static IApplicationBuilder UseSwaggerWithCvConfig(this IApplicationBuilder app)
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

            return app;
    }

    public static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services, string issuer)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity service API", Version = "v1" });

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
        
        return services;
    }
    
}