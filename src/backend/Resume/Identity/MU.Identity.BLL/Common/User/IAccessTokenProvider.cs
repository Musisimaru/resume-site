using System.Security.Claims;

namespace MU.Identity.BLL.Common.User;

public interface IAccessTokenProvider
{
    string? GetBearerToken();
    ClaimsPrincipal? GetUser();
}
