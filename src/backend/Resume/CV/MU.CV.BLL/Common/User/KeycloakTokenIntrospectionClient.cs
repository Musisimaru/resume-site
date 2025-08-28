using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace MU.CV.BLL.Common.User;

public class KeycloakTokenIntrospectionClient : ITokenIntrospectionClient
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        { PropertyNameCaseInsensitive = true };
    
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    private readonly string _realm;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public KeycloakTokenIntrospectionClient(HttpClient http, IMemoryCache cache,
        IConfiguration cfg)
    {
        _httpClient = http;
        _cache = cache;
        _realm = cfg["Keycloak:Realm"]!;
        _clientId = cfg["Keycloak:Introspect:ClientId"]!;
        _clientSecret = cfg["Keycloak:Introspect:ClientSecret"]!;
        
    }

    public async Task<IntrospectionResult?> Introspect(string token, CancellationToken ct = default)
    {
        // Кэш до exp (или 60 сек. по умолчанию)
        if(_cache.TryGetValue(token, out IntrospectionResult? cached))
            return cached;

        var ep = $"/realms/{_realm}/protocol/openid-connect/token/introspect";
        var data = new StringContent($"token={Uri.EscapeDataString(token)}", 
            Encoding.UTF8, "application/x-www-form-urlencoded");
        
        var basicAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}"));
        
        var request = new HttpRequestMessage(HttpMethod.Post, ep);
        request.Content = data;
        request.Headers.Add("Authorization", $"Basic {basicAuth}");
        request.Headers.Add("Accept", "application/json");
        
        var response = await _httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        
        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        var result = await JsonSerializer.DeserializeAsync<IntrospectionResult>(stream, 
            JsonOptions, ct);
        
        if (result is null || !result.active) return result;
        
        TimeSpan ttl = TimeSpan.FromSeconds(60);
        if (result.exp is { } exp)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var sec = Math.Max(1, exp - now);
            ttl = TimeSpan.FromSeconds(Math.Min(sec, 60));
        }
        
        _cache.Set(token, result, ttl);
        return result;
    }
}

public record IntrospectionResult
(
    bool active,
    long? exp,
    string? sub,
    string[]? role,
    string? email,
    string? given_name,
    string? family_name,
    string? company_name,
    string? company_logo,
    string? username,
    string? avatar
);
// Example
// {
//     "exp": 1756381083,
//     "iat": 1756380783,
//     "jti": "onrtac:9d3302ee-e376-ada9-39ae-a809fa64524a",
//     "iss": "http://localhost:9090/realms/resume-realm",
//     "aud": "resume-api",
//     "sub": "edd779cd-47c4-4aa2-b2b7-87f64b90e281",
//     "typ": "Bearer",
//     "azp": "resume-frontend",
//     "sid": "446ed3e1-6453-45e7-99c5-1f96c9049614",
//     "scope": "openid resume-frontend-dedicated",
//     "role": [
//     "employer"
//         ],
//     "company_name": "OZON",
//     "given_name": "User",
//     "family_name": "One",
//     "email": "employer01@example.com",
//     "client_id": "resume-frontend",
//     "username": "empl01",
//     "token_type": "Bearer",
//     "active": true
// }