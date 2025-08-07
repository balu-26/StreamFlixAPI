using Microsoft.EntityFrameworkCore;
using StreamFlixAPI.Data;
using StreamFlixAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddHttpClient<GeoLocationService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.WithOrigins(
        "http://localhost:3000",
        "https://gray-glacier-09eab180f.2.azurestaticapps.net"
    )
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// Program.cs (after app.MapControllers())
app.MapGet("/api/ping", () => Results.Ok("pong"));

app.MapGet("/api/db-check", async (AppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect ? Results.Ok("db-ok") : Results.StatusCode(500);
});

app.Run();
