using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MU.CV.BLL.Common;
using MU.CV.DAL.Entities.Note;

namespace MU.CV.API.Apis;

public static class MapNotesApi
{
    public static RouteGroupBuilder MapNotesApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/notes").HasApiVersion(1.0);

        api.MapGet("/", GetNotesByUserAsync);
        api.MapGet("/{noteId:guid}", GetNoteByUserAsync);
        api.MapPost("/", CreateNoteAsync);
        api.MapPut("/", UpdateNoteAsync);
        api.MapDelete("/", RemoveNoteAsync);
        
        return api;
    }

    private static Task<Results<Ok<Note>, NotFound>> GetNoteByUserAsync(Guid noteId)
    {
        throw new NotImplementedException();
    }
    
    private static async Task<Ok<IEnumerable<Note>>> GetNotesByUserAsync([FromServices] ICRUDService<NoteDAL> service)
    {
        var notes = (await service.GetAllAsync()).Select(note => new Note(note.Id, note.Title, note.Content));
        return TypedResults.Ok(notes);
    }

    private static Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> RemoveNoteAsync(Guid noteId)
    {
        throw new NotImplementedException();
    }

    private static Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> UpdateNoteAsync(Note updatedNote)
    {
        throw new NotImplementedException();
    }

    private static Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> CreateNoteAsync(Note createNote)
    {
        throw new NotImplementedException();
    }


}

public record Note(Guid Id, string Title, string Content);