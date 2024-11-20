using brainfreeze_new.Server.Models;
using brainfreeze_new.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Accessing the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DevConnection");

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("https://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddSingleton<UserService>();

// Registering the DbContext with the connection string
builder.Services.AddDbContext<ScoreboardDBContext>(options =>
    options.UseSqlServer(connectionString));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
