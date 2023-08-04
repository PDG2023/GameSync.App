global using FastEndpoints;
global using FastEndpoints.Security;

using FastEndpoints.Swagger;
using GameSync.Api.Identity;
using GameSync.Api.Persistence;
using GameSync.Business.BoardGamesGeek;
using GameSync.Business.Features.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<GameSyncContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddIdentityCore<User>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedEmail = true;
    x.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<GameSyncContext>().AddDefaultTokenProviders();

builder.Services.AddJWTBearerAuth("TokenSigningKey").AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCors();

builder.Services.AddSingleton<IGameSearcher, BoardGameGeekClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(configuration => configuration.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UsePathBase("/api");
app.UseFastEndpoints();
app.UseFileServer();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerGen();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider
        .GetRequiredService<GameSyncContext>()
        .Database.Migrate();
}

app.Run();

public partial class Program { }