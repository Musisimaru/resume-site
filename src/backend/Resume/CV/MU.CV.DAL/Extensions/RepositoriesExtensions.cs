using Microsoft.Extensions.DependencyInjection;
using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.Note;
using MU.CV.DAL.Repositories;
using MU.CV.DAL.Utils;

namespace MU.CV.DAL.Extensions;

public static class CVServiceRepositoriesCollectionExtensions
{
    public static IServiceCollection AddCVRepositories(this IServiceCollection services)
    {

        services.AddScoped<IBaseRepository<NoteDAL>, NotesRepository>();

        return services;
    } 
    
}