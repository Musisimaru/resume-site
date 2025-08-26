using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MU.CV.BLL.Common;
using MU.CV.BLL.Domains.Notes;
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
        api.MapDelete("/{noteId:guid}", RemoveNoteAsync);
        
        return api;
    }

    private static Guid DummyUser = Guid.Empty;


    private async static Task<Results<Ok<NoteDto>, NotFound>> GetNoteByUserAsync(Guid noteId,
        [FromServices] IDtoRead<NoteDto> readService)
    {
        // TODO: add filters by user
        var note = (await readService.GetByIdAsync(noteId));
        return TypedResults.Ok(note);
    }

    private static async Task<Ok<IEnumerable<NoteDto>>> GetNotesByUserAsync([FromServices] IDtoRead<NoteDto> readService)
    {
        // TODO: add filters by user
        var notes = (await readService.GetAllAsync());
        return TypedResults.Ok(notes);
    }

    private static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> RemoveNoteAsync(Guid noteId, 
        [FromServices] IDtoAuthorizedWrite<NoteDto> service)
    {
        await service.RemoveAsync(noteId);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> UpdateNoteAsync(NoteDto updatedNote, 
        [FromServices] IDtoAuthorizedWrite<NoteDto> service)
    {
        await service.UpdateAsync(updatedNote, DummyUser);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<Guid>, BadRequest<string>, ProblemHttpResult>> CreateNoteAsync(
        NoteDto createdNote, 
        [FromServices] IDtoAuthorizedWrite<NoteDto> service)
    {
        var newId = await service.CreateAsync(createdNote, DummyUser);
        return TypedResults.Ok(newId);
    }


}

