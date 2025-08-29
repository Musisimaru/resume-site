using Microsoft.Extensions.DependencyInjection;
using MU.CV.BLL.Common;
using MU.CV.BLL.Domains.Notes;
using MU.CV.BLL.Domains.Cv;
using MU.CV.BLL.Domains.JobExperience;
using MU.CV.DAL.Entities.Note;

namespace MU.CV.BLL.Extensions;

public static class CVServiceServicesCollectionExtensions
{
    public static IServiceCollection AddCVServices(this IServiceCollection services)
    {
        services.AddNotesServices();
        services.AddCvServices();
        services.AddJobExperienceServices();
        
        return services;
    }

}