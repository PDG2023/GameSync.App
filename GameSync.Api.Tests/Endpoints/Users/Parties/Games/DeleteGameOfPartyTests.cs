using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace Tests.Endpoints.Users.Parties.Games;

[Collection(GameSyncAppFactoryFixture.Name)]
public class DeleteGameOfPartyTests : TestsWithLoggedUser
{
    public DeleteGameOfPartyTests(GameSyncAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Deleting_existing_game_works()
    {
        // arrange
        var party = await Factory.CreateDefaultPartyAsync(UserId);
        var game = await Factory.CreateTestGameAsync(UserId);
        await Factory.CreatePartyGameAsync(party.Id, game.Id);

        // act
        var (response, _) = await DoDelete<Ok>(game.Id, party.Id);

        // assert
        response.EnsureSuccessStatusCode();

        var deletedPartyGame = await GetPartyGame(game.Id, party.Id);
        Assert.Null(deletedPartyGame);
    }

    [Fact]
    public async Task Deleting_other_user_party_game_produces_not_found()
    {
        // arrange
        var otherParty = await Factory.CreatePartyOfAnotherUserAsync();
        var game = await Factory.CreateTestGameAsync(otherParty.UserId);
        await Factory.CreatePartyGameAsync(otherParty.Id, game.Id);

        // act
        var (response, _) = await DoDelete<NotFound>(game.Id, otherParty.Id);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var deletedPartyGame = await GetPartyGame(game.Id, otherParty.Id);
        Assert.NotNull(deletedPartyGame);
    }

    [Fact]
    public async Task Deleting_game_of_non_existing_party_produces_not_found()
    {
        // arrange
        var party = await Factory.CreateDefaultPartyAsync(UserId);

        // act
        var (response, _) = await DoDelete<NotFound>(2095, 2409);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    private async Task<TestResult<TResponse>> DoDelete<TResponse>(int gameId, int partyId)
    {
        var req = new PartyGameRequest { GameId = gameId, PartyId = partyId };
        return await Client.DELETEAsync<DeleteGame.Endpoint, PartyGameRequest, TResponse>(req);
    }

    private async Task<PartyGame?> GetPartyGame(int gameId, int partyId)
    {
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        return await ctx.PartiesGames
            .FirstOrDefaultAsync(pg => pg.GameId == gameId && pg.PartyId == partyId);
    }
}
