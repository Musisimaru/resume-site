namespace MU.CV.BLL.Common.User;

public interface ITokenIntrospectionClient
{
    Task<IntrospectionResult?> Introspect(string token, CancellationToken ct = default);
}