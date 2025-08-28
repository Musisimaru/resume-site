using System.Security.Claims;
using MU.CV.BLL.Common.User;

namespace MU.CV.API.Accessors;


public sealed class HttpContextAccessTokenProvider(IHttpContextAccessor acc) : IAccessTokenProvider
{
    public string? GetBearerToken()
    {
        var auth = acc.HttpContext?.Request.Headers.Authorization.ToString();
        if (auth?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
            return auth.Substring("Bearer ".Length).Trim();
        return null;
    }

    public ClaimsPrincipal? GetUser() => acc.HttpContext?.User;
}

public static class AccessTokenProviderExtensions
{
    /// <summary>
    /// Add access token provider for ref libs
    /// [!IMPORTANT]: need declare `services.AddHttpContextAccessor();` before.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHttpContextAccessTokenProvider(this IServiceCollection services)
    {
        services.AddScoped<IAccessTokenProvider, HttpContextAccessTokenProvider>();
        
        return services;
    }
}