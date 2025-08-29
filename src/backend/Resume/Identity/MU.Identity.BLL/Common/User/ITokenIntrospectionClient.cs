namespace MU.Identity.BLL.Common.User;

public interface ITokenIntrospectionClient
{
    Task<IntrospectionResult?> Introspect(string token, CancellationToken ct = default);
}
