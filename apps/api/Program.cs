using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("API_PORT") ?? "4000";
builder.WebHost.UseUrls($"http://localhost:{port}");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddControllers();

var app = builder.Build();

app.UseCors();
app.MapControllers();

app.MapGet("/api/health", Ok<HealthResponse> () =>
{
    return TypedResults.Ok(new HealthResponse(
        "ok",
        "talent-skill-harvester-api",
        app.Environment.EnvironmentName.ToLowerInvariant()));
});

app.Run();

public sealed record HealthResponse(string Status, string Service, string Env);