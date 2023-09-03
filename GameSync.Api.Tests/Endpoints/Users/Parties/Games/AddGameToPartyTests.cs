using FastEndpoints;
using GameSync.Api;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Runtime.CompilerServices;
using Xunit;

namespace Tests.Endpoints.Users.Parties.Games;

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

        // Check that the game has not been added
        Assert.Null(await Fetch(request.PartyId, request.GameId));

    }

    [Fact]
    public async Task Add_existing_game_to_party_stores_it()
    {
        // arrange
        var request = await Factory.GetRequestToNonExistingPartyGame(UserId);

        // act
        var (response, _)
            = await Client.PUTAsync<AddGame.Endpoint, PartyGameRequest, Ok>(request);

        // assert
        response.EnsureSuccessStatusCode();

        Assert.NotNull(await Fetch(request.PartyId, request.GameId));
    }

    [Fact]
    public async Task Adding_twice_same_game_in_party_produces_bad_request()
    {
        // arrange
        var request = await Factory.GetRequestToNonExistingPartyGame(UserId);

        // act
        await Client.PUTAsync<AddGame.Endpoint, PartyGameRequest, Ok>(request);
        var (response, _) = await Client.PUTAsync<AddGame.Endpoint, PartyGameRequest, BadRequestWhateverError>(request);

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Check that the pair has not been deleted 
        Assert.NotNull(await Fetch(request.PartyId, request.GameId));

    }

    private async Task<PartyGame?> Fetch(int partyId, int gameId)
    {
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        return await ctx.PartiesGames
            .FirstOrDefaultAsync(x => x.PartyId == partyId && x.GameId == gameId);
    }
}
