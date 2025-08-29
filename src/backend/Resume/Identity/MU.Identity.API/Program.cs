using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MU.CV.Core;
using MU.Identity.API.Accessors;
using MU.Identity.API.Apis;
using MU.Identity.API.Extensions;
using MU.Identity.BLL.Common.User;

var builder = WebApplication.CreateBuilder(args);

builder.AddProblemProcessor();
builder.AddBasicServiceDefaults();

var issuer = builder.Configuration["Keycloak:Authority"];
if(issuer == null){ throw new ApplicationException("Keycloak:Authority is missing."); }

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth(issuer);

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Add authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = issuer;
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            
            // mappings
            NameClaimType = ClaimTypes.NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
        
        // logs for 401
        // TODO: only for development
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                ctx.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JWT")
                    .LogError(ctx.Exception, "Auth failed");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Add memory cache
builder.Services.AddMemoryCache();

// Add HTTP client
builder.Services.AddHttpClient();

// Add Identity services
builder.Services.AddHttpContextAccessTokenProvider();
builder.Services.AddCurrentUser();
//builder.Services.AddScoped<ITokenIntrospectionClient, KeycloakTokenIntrospectionClient>();
builder.Services.AddInrospection(builder.Configuration["Keycloak:BaseUrl"]);

builder.Services
    .AddApiVersioning(o =>
    {
        o.DefaultApiVersion = new ApiVersion(1, 0);
        o.AssumeDefaultVersionWhenUnspecified = true;
        o.ReportApiVersions = true;
        o.ApiVersionReader = new HeaderApiVersionReader("Api-Version");
    })
    // для Swagger с группировкой по версиям
    .AddApiExplorer(o =>
    {
        o.GroupNameFormat = "'v'VVV";
        o.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

app.AddProblemDetails();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithCvConfig();
}
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

// Map Identity API
var cvApi = app.NewVersionedApi();
cvApi.MapSelfIdentityApiV1();

app.Run();
