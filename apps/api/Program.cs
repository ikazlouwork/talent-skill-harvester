using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using TalentSkillHarvester.Api.Persistence;
using TalentSkillHarvester.Api.Persistence.Repositories;
using TalentSkillHarvester.Api.Storage;

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

var sqliteDbPath = Environment.GetEnvironmentVariable("SQLITE_DB_PATH");
if (string.IsNullOrWhiteSpace(sqliteDbPath))
{
    var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(dataDirectory);
    sqliteDbPath = Path.Combine(dataDirectory, "talent-skill-harvester.db");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={sqliteDbPath}"));

builder.Services.AddScoped<ISkillRepository, SqliteSkillRepository>();
builder.Services.AddScoped<IExtractionLogRepository, SqliteExtractionLogRepository>();
builder.Services.AddScoped<IApiStore, ApiStoreService>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    await DbSeeder.SeedAsync(dbContext);
}

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