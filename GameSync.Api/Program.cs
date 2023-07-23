using GameSync.Api.Persistence;
using GameSync.Business.Features.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<GameSyncContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddTransient<GameStoreSearcher>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
