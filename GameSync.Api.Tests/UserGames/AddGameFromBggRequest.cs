using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using Org.BouncyCastle.Crypto;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.UserGames;

[Collection("FullApp")]
public class AddGameFromBggRequest : TestsWithLoggedUser
{
    public AddGameFromBggRequest(GameSyncAppFactory factory) : base(factory)
    {
    }


    [Fact]
    public async Task Adding_non_existing_game_from_bgg_produces_not_found()
    {

        // arrange
        const int nonExistingId = 848965651;
        var addExistingGameRequest = new Endpoints.Users.Me.Games.AddGameFromBggRequest
        {
            IDs = new[] { nonExistingId }
        };

        // act
        var (response, result) = await Client.POSTAsync<AddGameFromBggEndpoint, Endpoints.Users.Me.Games.AddGameFromBggRequest, NotFound<List<int>>>(addExistingGameRequest);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(nonExistingId, Assert.Single(result.Value));

    }
}
