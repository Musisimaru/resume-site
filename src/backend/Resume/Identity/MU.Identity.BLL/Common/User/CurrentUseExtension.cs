using Microsoft.Extensions.DependencyInjection;
using MU.Identity.BLL.Exceptions;

namespace MU.Identity.BLL.Common.User;

public static class CurrentUseExtension{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ILazyRichUser, RichCurrentUser>();
        
        return services;
    }
}
