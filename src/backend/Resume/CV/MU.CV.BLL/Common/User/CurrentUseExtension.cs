using Microsoft.Extensions.DependencyInjection;
using MU.CV.BLL.Exceptions;

namespace MU.CV.BLL.Common.User;

public static class CurrentUseExtension{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ILazyRichUser, RichCurrentUser>();
        
        return services;
    }
}