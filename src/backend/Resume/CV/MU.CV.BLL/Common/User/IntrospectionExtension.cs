using Microsoft.Extensions.DependencyInjection;

namespace MU.CV.BLL.Common.User;

public static class InrospectionExtension
{
    public static IServiceCollection AddInrospection(this IServiceCollection services, string? introspectorBaseUrl)
    {
        services.AddHttpClient<ITokenIntrospectionClient, KeycloakTokenIntrospectionClient>(c =>
        {
            c.BaseAddress = new Uri(introspectorBaseUrl ?? throw new ArgumentNullException(nameof(introspectorBaseUrl)));
        });

        return services;
    }
    
}