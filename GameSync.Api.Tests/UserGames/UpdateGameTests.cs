using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace GameSync.Api.Tests.UserGames;

[Collection("FullApp")]
public class UpdateGameTests : TestsWithLoggedUser
{
    public UpdateGameTests(GameSyncAppFactory factory) : base(factory)
    {
    }


    [Fact]
    public async void Trying_to_update_non_existing_game_produces_404()
    {
        // arrange
        var updateRequest = new UpdateGameRequest { GameId = 10 };

        // act
        var (response, result) = await Client.POSTAsync<UpdateGameEndpoint, UpdateGameRequest, NotFound>(updateRequest);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
    }
}
