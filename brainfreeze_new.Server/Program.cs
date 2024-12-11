using brainfreeze_new.Server.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Accessing the connection string from appsettings.json
var frontendUrlPub = builder.Configuration["Frontend:UrlPub"];
var frontendUrlPriv = builder.Configuration["Frontend:UrlPriv"];
var connectionString = builder.Configuration.GetConnectionString("DevConnection");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(frontendUrlPub, frontendUrlPriv)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();


builder.Services.AddControllers();

// Registering the DbContext with the connection string
builder.Services.AddDbContext<ScoreboardDBContext>(options =>
    options.UseSqlServer(connectionString));

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Urls.Add("https://0.0.0.0:7005");



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
