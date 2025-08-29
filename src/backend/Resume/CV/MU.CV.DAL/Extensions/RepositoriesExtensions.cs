using Microsoft.Extensions.DependencyInjection;
using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.Note;
using MU.CV.DAL.Entities.Cv;
using MU.CV.DAL.Entities.JobExperience;
using MU.CV.DAL.Repositories;
using MU.CV.DAL.Utils;

namespace MU.CV.DAL.Extensions;

public static class CVServiceRepositoriesCollectionExtensions
{
    public static IServiceCollection AddCVRepositories(this IServiceCollection services)
    {

        services.AddScoped<IBaseRepository<NoteDAL>, NotesHistorianRepository>();
        services.AddScoped<IHistorianRepository<NoteDAL>, NotesHistorianRepository>();
        services.AddScoped<IProjectorRepository<NoteDAL>, NotesProjectorRepository>();

        services.AddScoped<IBaseRepository<CvDAL>, CvHistorianRepository>();
        services.AddScoped<IHistorianRepository<CvDAL>, CvHistorianRepository>();
        services.AddScoped<IProjectorRepository<CvDAL>, CvProjectorRepository>();

        services.AddScoped<IBaseRepository<JobExperienceDAL>, JobExperienceHistorianRepository>();
        services.AddScoped<IHistorianRepository<JobExperienceDAL>, JobExperienceHistorianRepository>();
        services.AddScoped<IProjectorRepository<JobExperienceDAL>, JobExperienceProjectorRepository>();

        return services;
    } 
    
}