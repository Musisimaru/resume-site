namespace MU.CV.API.Apis;

public static class CvAPI
{
    public static RouteGroupBuilder MapCvApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/cv").RequireAuthorization().HasApiVersion(1.0);

        api.MapGet("/", GetCVs);
        api.MapGet("/{cvId:guid}", GetCV);
        api.MapPost("/", CreateCV);
        api.MapPut("/", UpdateCV);
        api.MapDelete("/{cvId:guid}", RemoveCV);
        api.MapDelete("/decipher/{cvPath:string}", DecipherPath);
        
        api.MapGet("/{cvId:guid}/blocks", GetCVBlocks);
        api.MapGet("/{cvId:guid}/blocks/{blockId:guid}", GetCVBlock);
        api.MapPost("/{cvId:guid}/blocks/{blockId:guid}", CreateCVBlock);
        api.MapPut("/{cvId:guid}/blocks/{blockId:guid}", UpdateCVBlock);
        api.MapDelete("/{cvId:guid}/blocks/{blockId:guid}", RemoveCVBlock);

        return api;
    }

    private static Task DecipherPath(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task RemoveCV(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task CreateCV(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task UpdateCV(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task GetCVs(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task GetCV(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task RemoveCVBlock(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task UpdateCVBlock(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task CreateCVBlock(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task GetCVBlock(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private static Task GetCVBlocks(HttpContext context)
    {
        throw new NotImplementedException();
    }
}