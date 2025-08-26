using Microsoft.Extensions.DependencyInjection;
using MU.CV.BLL.Common;
using MU.CV.BLL.Domains.Notes;
using MU.CV.DAL.Entities.Note;

namespace MU.CV.BLL.Extensions;

public static class CVServiceServicesCollectionExtentions
{
    public static IServiceCollection AddCVServices(this IServiceCollection services)
    {
        services.AddScoped<ICRUDService<NoteDAL>, NotesCRUDService>();
        
        return services;
    }

}