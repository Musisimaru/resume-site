using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Hellang.Middleware.ProblemDetails;

namespace MU.CV.Core;

public static class Extensions
{
    public static IHostApplicationBuilder AddProblemProcessor(this IHostApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options =>
        {
            // Показывать детали исключения (сообщение/стек) только в Dev
            options.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();

            // Добавим общее обогащение перед отправкой (traceId, instance и т.п.)
            options.OnBeforeWriteDetails = (ctx, problem) =>
            {
                problem.Instance ??= ctx.Request.Path;
                problem.Extensions["traceId"] = Activity.Current?.Id ?? ctx.TraceIdentifier;
                // при желании: problem.Type/Title по умолчанию
            };

            // 404 для доменного "не найдено"
            options.Map<EntityNotFoundException>(ex =>
            {
                var pd = new ProblemDetails
                {
                    Title  = "Resource not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = ex.Message // в Prod можно убрать, оставить только Title
                };
                return pd;
            });

            // 400 для валидации (пример своей валидации/FluentValidation)
            // options.Map<ValidationException>(ex =>
            // {
            //     var pd = new ProblemDetails
            //     {
            //         Title  = "Validation failed",
            //         Status = StatusCodes.Status400BadRequest,
            //         Detail = "One or more validation errors occurred."
            //     };
            //
            //     // Словарь ошибок: { "Field": ["msg1","msg2"], ... }
            //     var errors = ex.Errors
            //         .GroupBy(e => e.PropertyName)
            //         .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            //
            //     pd.Extensions["errors"] = errors;
            //     return pd;
            // });

            // Простые маппинги: исключение -> статус
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
            options.MapToStatusCode<TimeoutException>(StatusCodes.Status504GatewayTimeout);

            // Фолбэк: всё остальное — 500
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        });

        return builder;
    }

    public static WebApplication AddProblemDetails(this WebApplication app)
    {
        // ВКЛЮЧИТЬ РАНО! Желательно самым первым middleware
        app.UseProblemDetails();

        // (опционально) Возвращать ProblemDetails и для «немых» 4xx/5xx без исключения,
        // например 404 на отсутствующий маршрут:
        app.UseStatusCodePages(async context =>
        {
            var http = context.HttpContext;
            // Не перезаписываем, если уже есть тело
            if (http.Response.HasStarted) return;

            var pd = new ProblemDetails
            {
                Status  = http.Response.StatusCode,
                Title   = http.Response.StatusCode switch
                {
                    404 => "Not Found",
                    401 => "Unauthorized",
                    403 => "Forbidden",
                    _   => "Error"
                },
                Instance = http.Request.Path
            };
            pd.Extensions["traceId"] = Activity.Current?.Id ?? http.TraceIdentifier;

            await Results.Problem(pd).ExecuteAsync(http);
        });

        return app;
    }
    
    
    public static IHostApplicationBuilder AddBasicServiceDefaults(this IHostApplicationBuilder builder)
    {
        // Default health checks assume the event bus and self health checks
        builder.AddDefaultHealthChecks();

        return builder;
    }
    
    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }
    
    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Uncomment the following line to enable the Prometheus endpoint (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
        // app.MapPrometheusScrapingEndpoint();

        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            app.MapHealthChecks("/health");

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }

        return app;
    }
    
}

// Примеры ваших исключений
sealed class EntityNotFoundException(string message) : Exception(message);

// sealed class ValidationException(IEnumerable<ValidationFailure> errors) : Exception("Validation failed")
// {
//     public IEnumerable<ValidationFailure> Errors { get; } = errors;
// }
// sealed record ValidationFailure(string PropertyName, string ErrorMessage);
