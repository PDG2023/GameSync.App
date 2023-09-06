using Bogus.DataSets;
using FakeItEasy;
using FastEndpoints;
using GameSync.Api.AuthMailServices;
using GameSync.Api.Extensions;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Tests.Mocks;
using Xunit;


namespace Tests;

[CollectionDefinition(Name)]
public class GameSyncAppFactoryFixture : ICollectionFixture<GameSyncAppFactory> 
{
    public const string Name = "FullApp";
}

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



    public async Task<CustomGame> CreateTestGameAsync(string userId)
    {
        var game = new CustomGame
        {
            MaxPlayer = 10,
            MinPlayer = 5,
            DurationMinute = 5,
            UserId = userId,
            MinAge = 5,
            Name = "game",
            Description = "Game's description",
            ImageUrl = "img"
        };

        using var scope = Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        await ctx.CustomGames.AddAsync(game);
        var n = await ctx.SaveChangesAsync();
        return game;
    }

    public async Task<PartyCustomGame> CreatePartyGameOfOtherUserAsync(List<Vote>? votes = null, string? invitationToken = null)
    {
        var party = await CreatePartyOfAnotherUserAsync(invitationToken);
        var game = await CreateTestGameAsync(party.UserId);
        return await CreatePartyGameAsync(party.Id, game.Id, votes);
    }

    public async Task CreateUnconfirmedUserAsync(string mail, string username, string password)
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

    public async Task<string> CreateConfirmedUserAsync(string mail, string username, string password)
    {
        await CreateUnconfirmedUserAsync(mail, username, password);
        using var scope = Services.CreateScope();
        var manager = scope.Resolve<UserManager<User>>();
        var user = await manager.FindByEmailAsync(mail);
        var confirmationToken = await manager.GenerateEmailConfirmationTokenAsync(user!);
        await manager.ConfirmEmailAsync(user!, confirmationToken);
        return user!.Id;

    }

    public async Task<Party> CreatePartyAsync(Party party)
    {
        using var scope = Services.CreateScope();
        var manager = scope.Resolve<GameSyncContext>();
        await manager.Parties.AddAsync(party);
        await manager.SaveChangesAsync();
        return party;
    }

    public async Task<Party> CreatePartyOfAnotherUserAsync(string? invitationToken = null)
    {
        var userId = await CreateConfirmedUserAsync(
            new Internet().Email(),
            new Internet().UserName(),
            "MuCkT*sgb2TB4!4P^r7cwRx");
        return await CreateDefaultPartyAsync(userId, invitationToken);
    }

    public async Task<Party> CreateDefaultPartyAsync(string userId, string? invitationToken = null) => await CreatePartyAsync(new Party
    {
        DateTime = DateTime.Now.AddDays(1),
        Location = "...",
        Name = "...",
        UserId = userId,
        InvitationToken = invitationToken
    });

    public async Task<PartyCustomGame> CreatePartyGameAsync(int partyId, int gameId, List<Vote>? votes = null)
    {
        using var scope = Services.CreateScope();
        var manager = scope.Resolve<GameSyncContext>();
        var entity = await manager.PartyCustomGames.AddAsync(new PartyCustomGame
        {
            GameId = gameId,
            PartyId = partyId,
            Votes = votes ?? new List<Vote>()
        });
        await manager.SaveChangesAsync();
        return entity.Entity;
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

    private static void SetupMockMailService(IServiceCollection services)
    {
        services.RemoveService<IConfirmationEmailSender>();
        services.AddSingleton<IConfirmationEmailSender>(new MockMailService(false));

        services.RemoveService<IPasswordResetMailSender>();
        services.AddSingleton<IPasswordResetMailSender>(new MockMailService(false));
    }

    private static void SetupFakeConfiguration(IServiceCollection services)
    {
        // Create a mock config which returns a temp password signing key for the jwt token
        var fakeConfig = A.Fake<IConfiguration>();
        A.CallTo(() => fakeConfig["Jwt:SignKey"]).Returns("yD2%#M3meB@nB6Q$%bFbL4naAEjpHdWSQXyUexgJimSkQrc6PMppoTN%");
        A.CallTo(() => fakeConfig["Jwt:Issuer"]).Returns("https://localhost");
        A.CallTo(() => fakeConfig["FrontPathToInvitedParty"]).Returns("{InvitationToken}");
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
