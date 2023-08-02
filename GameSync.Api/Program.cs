using GameSync.Api.Persistence;
using GameSync.Business.BoardGamesGeek;
using GameSync.Business.Features.Search;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GameSyncContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddSingleton<IGameSearcher, BoardGameGeekClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
    app.UseCors(configuration => configuration.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UsePathBase("/api");
app.UseFileServer();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider
        .GetRequiredService<GameSyncContext>()
        .Database.Migrate();
}

app.MapControllers();
app.Run();

public partial class Program { }