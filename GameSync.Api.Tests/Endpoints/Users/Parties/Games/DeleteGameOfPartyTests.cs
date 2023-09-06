using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.PartyGame;
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
        var pg = await Factory.CreatePartyGameAsync(party.Id, game.Id);

        // act
        var (response, _) = await DoDelete<Ok>(pg.Id);

        // assert
        response.EnsureSuccessStatusCode();

        var deletedPartyGame = await GetPartyGame(pg.Id);
        Assert.Null(deletedPartyGame);
    }

    [Fact]
    public async Task Deleting_other_user_party_game_produces_not_found()
    {
        // arrange
        var otherParty = await Factory.CreatePartyOfAnotherUserAsync();
        var game = await Factory.CreateTestGameAsync(otherParty.UserId);
        var pg = await Factory.CreatePartyGameAsync(otherParty.Id, game.Id);

        // act
        var (response, _) = await DoDelete<NotFound>(pg.Id);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var deletedPartyGame = await GetPartyGame(pg.Id);
        Assert.NotNull(deletedPartyGame);
    }

    [Fact]
    public async Task Deleting_game_of_non_existing_party_produces_not_found()
    {

        // act
        var (response, _) = await DoDelete<NotFound>(45487874);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    private async Task<TestResult<TResponse>> DoDelete<TResponse>(int pgId)
    {
        var req = new RequestToIdentifiableObject { Id = pgId };
        return await Client.DELETEAsync<DeletePartyGame.Endpoint, RequestToIdentifiableObject, TResponse>(req);
    }

    private async Task<PartyGame?> GetPartyGame(int id)
    {
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        return await ctx.PartiesGames
            .FirstOrDefaultAsync(pg => pg.Id == id);
    }
}
