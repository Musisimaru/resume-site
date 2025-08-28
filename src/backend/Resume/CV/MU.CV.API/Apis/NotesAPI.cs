using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MU.CV.BLL.Common;
using MU.CV.BLL.Common.User;
using MU.CV.BLL.Domains.Notes;
using MU.CV.DAL.Entities.Note;

namespace MU.CV.API.Apis;

public static class MapNotesApi
{
    public static RouteGroupBuilder MapNotesApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/notes").RequireAuthorization().HasApiVersion(1.0);

        api.MapGet("/", GetNotesByUserAsync);
        api.MapGet("/{noteId:guid}", GetNoteByUserAsync);
        api.MapPost("/", CreateNoteAsync);
        api.MapPut("/", UpdateNoteAsync);
        api.MapDelete("/{noteId:guid}", RemoveNoteAsync);
        
        return api;
    }

    private async static Task<Results<Ok<NoteDto>, NotFound>> GetNoteByUserAsync(Guid noteId,
        [FromServices] IDtoRead<NoteDto> readService)
    {
        // TODO: add filters by user
        var note = (await readService.GetByIdAsync(noteId));
        return TypedResults.Ok(note);
    }

    private static async Task<Ok<IEnumerable<NoteDto>>> GetNotesByUserAsync(
        [FromServices] IDtoRead<NoteDto> readService, 
        [FromServices] ICurrentUser user)
    {
        // TODO: add filters by user
        
        var notes = (await readService.GetAllAsync());
        return TypedResults.Ok(notes);
    }

    private static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> RemoveNoteAsync(Guid noteId, 
        [FromServices] IDtoWrite<NoteDto> service)
    {
        await service.RemoveAsync(noteId);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> UpdateNoteAsync(NoteDto updatedNote, 
        [FromServices] IDtoWrite<NoteDto> service,
        [FromServices] ICurrentUser user)
    {
        await service.UpdateAsync(updatedNote);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<Guid>, BadRequest<string>, ProblemHttpResult>> CreateNoteAsync(
        NoteDto createdNote, 
        [FromServices] IDtoWrite<NoteDto> service,
        [FromServices] ICurrentUser user)
    {
        var newId = await service.CreateAsync(createdNote);
        return TypedResults.Ok(newId);
    }


}

