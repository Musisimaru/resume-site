using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MU.CV.DAL.DataContext;
using MU.CV.DAL.Utils;

namespace MU.CV.DAL.Extensions;

public static class CVServiceDataContextsExtension
{
    public static IServiceCollection AddCVDataContext(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IUnitOfWork, CVPGUnitOfWork>();
        services.AddDbContext<CVDbContext>(options =>
        {
            options.UseNpgsql(connectionString,
                sqlServerOptions => sqlServerOptions.CommandTimeout((int)TimeSpan.FromMinutes(CVDbContext.COMMAND_TIMEOUT__MINUTES).TotalSeconds));
        });

        return services;
    }
}