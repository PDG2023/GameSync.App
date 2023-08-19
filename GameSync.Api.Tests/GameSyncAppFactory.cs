using FakeItEasy;
using FastEndpoints;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Tests.Identity;
using GameSync.Business.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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

    public async Task CreateUnconfirmedUser(string mail, string username, string password)
    {
        var user = new User
        {
            Email = mail,
            UserName = username
        };

        using var scope = Services.CreateScope();
        var manager = scope.Resolve<UserManager<User>>();
        await manager.CreateAsync(user, password);
    }

    public async Task<string> CreateConfirmedUser(string mail, string username, string password)
    {
        await CreateUnconfirmedUser(mail, username, password);
        using var scope = Services.CreateScope();
        var manager = scope.Resolve<UserManager<User>>();
        var user = await manager.FindByEmailAsync(mail);
        var confirmationToken = await manager.GenerateEmailConfirmationTokenAsync(user!);
        await manager.ConfirmEmailAsync(user!, confirmationToken);
        return user.Id;

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
            SetupMockMailService(services);
        });
    }

    private void SetupMockMailService(IServiceCollection services)
    {
        services.RemoveService<IConfirmationEmailSender>();
        services.AddSingleton<IConfirmationEmailSender>(new MockMailService(false));
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
