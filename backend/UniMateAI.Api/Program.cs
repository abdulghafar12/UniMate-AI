using Microsoft.EntityFrameworkCore;
using UniMateAI.Api.Services;
using UniMateBackend.Data;
using UniMateBackend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
var dataDirectory = Path.Combine(projectRoot, "Data");
Directory.CreateDirectory(dataDirectory);
var dbPath = Path.Combine(dataDirectory, "unimate.db");
builder.Configuration["ConnectionStrings:UniMateDatabase"] = $"Data Source={dbPath}";

builder.WebHost.UseUrls($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT") ?? "5001"}");

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<UniMateDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("UniMateDatabase")));

// AI Service
builder.Services.AddHttpClient<IAiService, AiService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("UniMateFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Create SQLite database automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<UniMateDbContext>();

    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("UniMateFrontend");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "ok",
        service = "UniMate Backend",
        version = "1.0"
    });
});

app.Run();
