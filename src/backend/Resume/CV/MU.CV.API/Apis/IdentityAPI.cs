using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MU.CV.BLL.Common.User;

namespace MU.CV.API.Apis;

public static class SelfIdentityAPI
{
    public static RouteGroupBuilder MapSelfIdentityApiV1(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/me").RequireAuthorization().HasApiVersion(1.0);

        api.MapGet("/profile", GetUserProfile);

        return api;
    }
    
    private static async Task<Results<Ok<ProfileDto>, NotFound>> GetUserProfile(
        [FromServices] ILazyRichUser lazyUser)
    {
        return TypedResults.Ok(new ProfileDto(await lazyUser.UseAsync()));
    }

}

record ProfileDto
{
    public ProfileDto(ILazyRichUser lazyUser)
    {
        Id = lazyUser.Id;
        Name = lazyUser.Name;
        LastName = lazyUser.LastName;
        Email = lazyUser.Email;
        CompanyName = lazyUser.CompanyName;
        CompanyLogoUrl = lazyUser.CompanyLogoUrl;
        AvatarUrl = lazyUser.AvatarUrl;
    }
    public Guid Id { get; set; }
    public string Name { get; }
    public string LastName { get; }
    public string Email { get; }
    public string CompanyName { get; }
    public string CompanyLogoUrl { get; }
    public string AvatarUrl { get; }
}