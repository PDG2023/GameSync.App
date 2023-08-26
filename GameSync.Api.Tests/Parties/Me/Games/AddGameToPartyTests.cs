using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.Parties.Me.Games;

[Collection("FullApp")]
public class AddGameToPartyTests : TestsWithLoggedUser
{
    public AddGameToPartyTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Add_non_existing_game_of_user_collection_produces_not_found()
    {
        // arrange
        var party = await Factory.CreateDefaultParty(UserId);
        var request = new PartyGameRequest { GameId = 918528, PartyId = party.Id };

        // act

        var (response, _)
            = await Client.PUTAsync<AddGame.Endpoint, PartyGameRequest, NotFound>(request);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Add_existing_game_to_party_stores_it()
    {
        // arrange
        var party = await Factory.CreateDefaultParty(UserId);
        var game = await Factory.CreateTestGame(UserId);
        var request = new PartyGameRequest
        {
            GameId = game.Id,
            PartyId = party.Id
        };

        // act
        var (response, _)
            = await Client.PUTAsync<AddGame.Endpoint, PartyGameRequest, Ok>(request);

        // assert
        response.EnsureSuccessStatusCode();

        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var partyGame = await ctx.PartiesGames
            .FirstOrDefaultAsync(x => x.PartyId == request.PartyId && x.GameId == request.GameId);

        Assert.NotNull(partyGame);


    }

}
