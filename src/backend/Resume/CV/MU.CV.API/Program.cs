using System.Diagnostics;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MU.CV.API.Apis;
// using MU.CV.API.ExceptionHandler;
using Hellang.Middleware.ProblemDetails;
using MU.CV.API.Extensions;
using MU.CV.BLL.Extensions;
using MU.CV.DAL.Extensions;

namespace MU.CV.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddProblemProcessor();
        builder.AddBasicServiceDefaults();

        builder.Services.AddCVDataContext(builder.Configuration.GetConnectionString("CVDb"));
        builder.Services.AddCVRepositories();
        builder.Services.AddCVServices();
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
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
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapDefaultEndpoints();
        
        var orders = app.NewVersionedApi();
        orders.MapNotesApiV1();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                        new WeatherForecast
                        {
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            TemperatureC = Random.Shared.Next(-20, 55),
                            Summary = summaries[Random.Shared.Next(summaries.Length)]
                        })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

        app.Run();
    }
}
