using Microsoft.Extensions.DependencyInjection;
using MU.CV.BLL.Common;
using MU.CV.BLL.Common.Models;
using MU.CV.BLL.Common.User;
using MU.CV.DAL.Common;
using MU.CV.DAL.Entities.Note;
using MU.CV.DAL.Utils;

namespace MU.CV.BLL.Domains.Notes;

public class NotesAuthorizedWriteService(IHistorianRepository<NoteDAL> repo, IUnitOfWork uow, ICurrentUser user)
    : BaseAuthorizedWrite<NoteDAL, NoteDto>(repo, uow, user,
        (stored, edited) =>
        {
            stored.Title = edited.Title;
            stored.Content = edited.Content;
        });

public class NotesAuthorizedReadService(IProjectorRepository<NoteDAL> repo, ICurrentUser user)
    : BaseAuthorizedRead<NoteDAL, NoteDto>(repo, user, (dal => new NoteDto(dal.Id, dal.Title, dal.Content)));

public static class NotesExtentions
{
    public static IServiceCollection AddNotesServices(this IServiceCollection services)
    {
        services.AddScoped<IDtoWrite<NoteDto>, NotesAuthorizedWriteService>();
        services.AddScoped<IDtoRead<NoteDto>, NotesAuthorizedReadService>();
        
        return services;
    }

}


public record NoteDto(Guid Id, string Title, string Content) : ValueBaseDtoEntity<NoteDAL>(Id)
{
       public NoteDto() : this(Guid.Empty, string.Empty, string.Empty) { }

       public override NoteDAL ToDbEntity()
       {
           return new NoteDAL(){Id = Id, Title = Title, Content = Content };
       }
}