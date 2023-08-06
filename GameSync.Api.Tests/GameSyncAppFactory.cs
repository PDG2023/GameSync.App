using FakeItEasy;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Xunit;

namespace GameSync.Api.Tests;

[CollectionDefinition("FullApp")]
public class GameSyncAppFactoryFixture : ICollectionFixture<GameSyncAppFactory> { }

public class GameSyncAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{

    private readonly PostgreSqlContainer _postgreSqlContainer;

    public GameSyncAppFactory()
    {

         _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .WithAutoRemove(true)
        .Build();
        
    }


    public async Task InitializeAsync() => await _postgreSqlContainer.StartAsync();

    public new async Task DisposeAsync() => await _postgreSqlContainer.DisposeAsync().AsTask();

    // Override the actual dbcontext as to not write to the actual databases and use the temp test container
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveService<DbContextOptions<GameSyncContext>>();
            AddContextWithTempContainerAsSource(services);
            EnsureSchemaCreated(services);
            SetupFakeConfiguration(services);
        });
    }

    private static void SetupFakeConfiguration(IServiceCollection services)
    {
        // Create a mock config which returns a temp password signing key for the jwt token
        const string mockKey = "yD2%#M3meB@nB6Q$%bFbL4naAEjpHdWSQXyUexgJimSkQrc6PMppoTN%";
        var fakeConfig = A.Fake<IConfiguration>();
        A.CallTo(() => fakeConfig["Jwt:SignKey"]).Returns(mockKey);
        A.CallTo(() => fakeConfig["Jwt:Issuer"]).Returns("https://localhost");
        services.RemoveService<IConfiguration>();
        services.AddSingleton(fakeConfig);
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
        context.Database.Migrate();
    }

}
