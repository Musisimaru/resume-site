using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MU.CV.BLL.Common.User;

public class CurrentUser : ICurrentUser
{
    private Guid _id;

    public Guid Id
    {
        get { return _id; }
    }

    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public CurrentUser(IHttpContextAccessor accessor)
    {
        var user = accessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            Guid.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out _id);
            Name = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            LastName = user.FindFirst(ClaimTypes.NameIdentifier)?.Value?? string.Empty;
        }
    }
}

public static class CurrentUseExtension{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<ClaimsPrincipal>(sp =>
            sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.User
            ?? new ClaimsPrincipal());
        services.AddScoped<ICurrentUser, CurrentUser>();
        
        return services;
    }
}

public interface ICurrentUser
{
    public Guid Id { get; }
    public string Name { get; set; }
    public string LastName { get; set; }

}