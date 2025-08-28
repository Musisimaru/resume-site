using System.Security.Claims;

namespace MU.CV.BLL.Common.User;

public interface IAccessTokenProvider
{
    string? GetBearerToken();
    ClaimsPrincipal? GetUser();
}