using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Runtime.InteropServices;
using Testcontainers.PostgreSql;
using Xunit;

namespace GameSync.Api.Tests;

public class GameSyncAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _postgreSqlContainer;
    private readonly WebApplicationFactoryClientOptions _opt;

    public GameSyncAppFactory()
    {

         _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();


        _opt = new WebApplicationFactoryClientOptions();
        
    }


    public async Task InitializeAsync() => await _postgreSqlContainer.StartAsync();

    public new async Task DisposeAsync() => await _postgreSqlContainer.DisposeAsync().AsTask();

    // Override the actual dbcontext as to not write to the actual databases and use the temp test container
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            RemoveDbContext(services);
            AddContextWithTempContainerAsSource(services);
            EnsureSchemaCreated(services);
        });
    }

    private void AddContextWithTempContainerAsSource(IServiceCollection services)
    {
        services.AddDbContext<GameSyncContext>(options => { options.UseNpgsql(_postgreSqlContainer.GetConnectionString()); });
    }

    private static void EnsureSchemaCreated(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<GameSyncContext>();
        context.Database.EnsureCreated();
    }

    private static void RemoveDbContext(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<GameSyncContext>));
        if (descriptor != null) services.Remove(descriptor);
    }


    public HttpClient Client => CreateClient(_opt);

}
