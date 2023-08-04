global using FastEndpoints;
global using FastEndpoints.Security;

using FastEndpoints.Swagger;
using GameSync.Api.Identity;
using GameSync.Api.Persistence;
using GameSync.Business.BoardGamesGeek;
using GameSync.Business.Features.Search;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(configuration => configuration.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");
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