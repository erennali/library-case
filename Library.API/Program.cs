using Library.Persistence;
using Microsoft.EntityFrameworkCore;
using Library.Application;
using Library.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Library API",
        Version = "v1",
        Description = "RISE Library Management System API"
    });
});

// Application & Persistence DI
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);

// Configure JWT Authentication
builder.Services.Configure<Library.Application.Configuration.JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.Configure<Library.Application.Configuration.EmailSettings>(
    builder.Configuration.GetSection("Email"));

builder.Services.Configure<Library.Application.Configuration.AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "database", tags: new[] { "database", "sql", "ready" })
    .AddCheck("storage", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy("Storage is healthy"), tags: new[] { "storage" });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.MapControllers();

// Map Health Checks
app.MapHealthChecks("/health");

app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}