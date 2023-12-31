

global using FastEndpoints;
global using FastEndpoints.Security;


using FastEndpoints.Swagger;
using GameSync.Api;
using GameSync.Api.AuthMailServices;
using GameSync.Api.BoardGameGeek;
using GameSync.Api.MailSender;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(x => x.ShortSchemaNames = true);
builder.Services.AddMemoryCache();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<GameSyncContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
    options.UseLazyLoadingProxies();
});

builder.Services.AddIdentityCore<User>(x =>
    {
        x.User.RequireUniqueEmail = true;
        x.SignIn.RequireConfirmedEmail = true;
        x.Password.RequiredLength = 8;
    })
    .AddErrorDescriber<FrenchIdentityErrorDescriber>()
    .AddUserManager<UserManager<User>>()
    .AddSignInManager()
    .AddEntityFrameworkStores<GameSyncContext>()
    .AddDefaultTokenProviders();

builder.Services.Replace(ServiceDescriptor.Scoped<IUserValidator<User>, AllowDuplicateUserNameUserValidator<User>>());

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddJWTBearerAuth(
    builder.Configuration["Jwt:SignKey"], 
    JWTBearer.TokenSigningStyle.Symmetric,
    tokenValidation: (validationParam) =>
    {
        validationParam.ValidateIssuerSigningKey = true;
        validationParam.ValidAudience = builder.Configuration["Jwt:Issuer"];
        validationParam.ValidateAudience = true;

        validationParam.ValidIssuer = builder.Configuration["Jwt:Issuer"];
        validationParam.ValidateIssuer = true;
        validationParam.RequireExpirationTime = true;
        validationParam.RequireSignedTokens = true;

    });

builder.Services.AddAuthorization();
builder.Services.AddCors();
builder.Services.AddSingleton<BoardGameGeekClient, CachedBoardGameGeekClient>();

if (!builder.Environment.IsProduction())
{
    builder.Services.AddSingleton<IMailSender, SmtpMailSender>();
}
else
{
    builder.Services.AddSingleton<IMailSender, AzureMailSender>();
}

builder.Services.AddSingleton<IConfirmationEmailSender, AuthMailService>();
builder.Services.AddSingleton<IPasswordResetMailSender, AuthMailService>();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseCors(configuration => configuration.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
}

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Errors.ResponseBuilder = (failures, context, statusCode) =>
    {

        return new BadRequestWhateverError(failures);

    };
});
app.UseFileServer();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerGen();
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider
        .GetRequiredService<GameSyncContext>()
        .Database.Migrate();
}

app.Run();

public partial class Program { }
