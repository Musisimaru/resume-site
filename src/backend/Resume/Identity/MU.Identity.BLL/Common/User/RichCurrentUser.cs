using Microsoft.AspNetCore.Http;
using MU.Identity.BLL.Exceptions;

namespace MU.Identity.BLL.Common.User;

public class RichCurrentUser : CurrentUser, ILazyRichUser
{
    private readonly ITokenIntrospectionClient _tokenIntrospectionClient;

    public string Name { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string CompanyName { get; private set; }
    public string CompanyLogoUrl { get; private set; }
    public string AvatarUrl { get; private set; }

    private readonly Lazy<Task<RichCurrentUser>> _init;
    private readonly string jwt;

    public RichCurrentUser(IAccessTokenProvider accessor, ITokenIntrospectionClient tokenIntrospectionClient)
        : base(accessor)
    {
        _tokenIntrospectionClient = tokenIntrospectionClient;
        jwt = accessor.GetBearerToken() ?? throw new ArgumentNullException(nameof(accessor.GetBearerToken));
        
        _init = new(() => Enrich());
    }

    private async Task<RichCurrentUser> Enrich(CancellationToken ct = default)
    {
        var info = await _tokenIntrospectionClient.Introspect(jwt, ct);
        if (info is null || !info.active) throw new IntrospectionException("User token is not active");

        Name = info.given_name ?? string.Empty;
        LastName = info.family_name ?? string.Empty;
        Email = info.email ?? string.Empty;
        CompanyName = info.company_name ?? string.Empty;
        CompanyLogoUrl = info.company_logo ?? string.Empty;
        AvatarUrl = info.avatar ?? string.Empty;
        
        return this;
    }
    
    public async Task<ILazyRichUser> UseAsync()
    {
        return await _init.Value;
    }
}

public interface IUser
{
    string Name { get; }
    public string LastName { get; }
    public string Email { get; }
    public string CompanyName { get; }
    public string CompanyLogoUrl { get; }
    public string AvatarUrl { get; }
}

public interface ILazyRichUser : IUser, ICurrentUser
{
    public Task<ILazyRichUser> UseAsync();
}
