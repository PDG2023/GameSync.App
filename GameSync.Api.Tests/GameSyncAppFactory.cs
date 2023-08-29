using Bogus.DataSets;
using FakeItEasy;
using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;
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



    public async Task<Game> CreateTestGame(string userId)
    {
        var game = new Game
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
        await ctx.Games.AddAsync(game);
        await ctx.SaveChangesAsync();
        return game;
    }

    public async Task<PartyGame> CreateFullPartyGameAsync(List<Vote>? votes = null)
    {
        var party = await CreatePartyOfAnotherUser();
        var game = await CreateTestGame(party.UserId);
        await CreatePartyGame(party.Id, game.Id, votes);
        return new PartyGame { GameId = game.Id, PartyId = party.Id };
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
        return user!.Id;

    }

    public async Task<Party> CreateParty(Party party)
    {
        using var scope = Services.CreateScope();
        var manager = scope.Resolve<GameSyncContext>();
        await manager.Parties.AddAsync(party);
        await manager.SaveChangesAsync();
        return party;
    }

    public async Task<Party> CreatePartyOfAnotherUser()
    {
        var userId = await CreateConfirmedUser(
            new Internet().Email(), 
            new Internet().UserName(), 
            "MuCkT*sgb2TB4!4P^r7cwRx");
        return await CreateDefaultParty(userId);
    }

    public async Task<Party> CreateDefaultParty(string userId) => await CreateParty(new Party
    {
        DateTime = DateTime.Now.AddDays(1),
        Location = "...",
        Name = "...",
        UserId = userId
    });

    public async Task CreatePartyGame(int partyId, int gameId, List<Vote>? votes = null)
    {
        using var scope = Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        await ctx.PartiesGames.AddAsync(new PartyGame
        {
            GameId = gameId,
            PartyId = partyId,
            Votes = votes
        });
        await ctx.SaveChangesAsync();
    }


    public async Task<PartyGameRequest> GetRequestToNonExistingPartyGame(string userId)
    {
        var party = await CreateDefaultParty(userId);
        var game = await CreateTestGame(userId);
        return new PartyGameRequest
        {
            GameId = game.Id,
            PartyId = party.Id
        };
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

        services.RemoveService<IPasswordResetMailSenderAsync>();
        services.AddSingleton<IPasswordResetMailSenderAsync>(new MockMailService(false));
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
