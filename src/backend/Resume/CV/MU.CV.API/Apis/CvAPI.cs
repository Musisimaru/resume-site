using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MU.CV.BLL.Common;
using MU.CV.BLL.Domains.Cv;
using MU.CV.BLL.Domains.JobExperience;

namespace MU.CV.API.Apis;

public static class CvAPI
{
    public static RouteGroupBuilder MapCvApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/cv").RequireAuthorization().HasApiVersion(1.0);

        app.MapGet("/api/cv", GetCVs).AllowAnonymous().HasApiVersion(1.0);
        app.MapGet("/api/cv/{cvId:guid}", GetCV).AllowAnonymous().HasApiVersion(1.0);
        api.MapPost("/", CreateCV);
        api.MapPut("/", UpdateCV);
        api.MapDelete("/{cvId:guid}", RemoveCV);
        app.MapGet("/api/cv/decipher/{cvPath}", DecipherPath).AllowAnonymous().HasApiVersion(1.0);
        
        app.MapGet("/api/cv/{cvId:guid}/blocks", GetJobExperiences).AllowAnonymous().HasApiVersion(1.0);
        app.MapGet("/api/cv/{cvId:guid}/blocks/{blockId:guid}", GetJobExperience).AllowAnonymous().HasApiVersion(1.0);
        api.MapPost("/{cvId:guid}/blocks/{blockId:guid}", CreateJobExperience);
        api.MapPut("/{cvId:guid}/blocks/{blockId:guid}", UpdateJobExperience);
        api.MapDelete("/{cvId:guid}/blocks/{blockId:guid}", RemoveJobExperience);

        return api;
    }

    private static async Task<Results<Ok<CvDto>, NotFound>> DecipherPath(string cvPath, [FromServices] ICvReadByPath readByPath, CancellationToken ct)
    {
        var cv = await readByPath.GetByUniquePathAsync(cvPath, ct);
        if (cv is null) return TypedResults.NotFound();
        return TypedResults.Ok(cv);
    }

    private static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> RemoveCV(Guid cvId, [FromServices] IDtoWrite<CvDto> writeService)
    {
        await writeService.RemoveAsync(cvId);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<Guid>, BadRequest<string>, ProblemHttpResult>> CreateCV([FromBody] CvDto createdCv, [FromServices] IDtoWrite<CvDto> writeService)
    {
        var newId = await writeService.CreateAsync(createdCv);
        return TypedResults.Ok(newId);
    }

    private static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> UpdateCV([FromBody] CvDto updatedCv, [FromServices] IDtoWrite<CvDto> writeService)
    {
        await writeService.UpdateAsync(updatedCv);
        return TypedResults.Ok();
    }

    private static async Task<Ok<IEnumerable<CvDto>>> GetCVs([FromServices] IDtoRead<CvDto> readService)
    {
        var items = await readService.GetAllAsync();
        return TypedResults.Ok(items);
    }

    private static async Task<Results<Ok<CvDto>, NotFound>> GetCV(Guid cvId, [FromServices] IDtoRead<CvDto> readService)
    {
        var cv = await readService.GetByIdAsync(cvId);
        return cv is null ? TypedResults.NotFound() : TypedResults.Ok(cv);
    }

    private static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> RemoveJobExperience(Guid cvId, Guid blockId, [FromServices] IDtoWrite<JobExperienceDto> writeService)
    {
        await writeService.RemoveAsync(blockId);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok, BadRequest<string>, ProblemHttpResult>> UpdateJobExperience(Guid cvId, Guid blockId, [FromBody] JobExperienceDto updated, [FromServices] IDtoWrite<JobExperienceDto> writeService)
    {
        updated = updated with { Id = blockId, CvId = cvId };
        await writeService.UpdateAsync(updated);
        return TypedResults.Ok();
    }

    private static async Task<Results<Ok<Guid>, BadRequest<string>, ProblemHttpResult>> CreateJobExperience(Guid cvId, Guid blockId, [FromBody] JobExperienceDto created, [FromServices] IDtoWrite<JobExperienceDto> writeService)
    {
        created = created with { CvId = cvId };
        var newId = await writeService.CreateAsync(created);
        return TypedResults.Ok(newId);
    }

    private static async Task<Results<Ok<JobExperienceDto>, NotFound>> GetJobExperience(Guid cvId, Guid blockId, [FromServices] IDtoRead<JobExperienceDto> readService)
    {
        var block = await readService.GetByIdAsync(blockId);
        return block is null ? TypedResults.NotFound() : TypedResults.Ok(block);
    }

    private static async Task<Ok<IEnumerable<JobExperienceDto>>> GetJobExperiences(Guid cvId, [FromServices] IDtoRead<JobExperienceDto> readService)
    {
        var blocks = await readService.GetAllAsync();
        return TypedResults.Ok(blocks.Where(b => b.CvId == cvId));
    }
}