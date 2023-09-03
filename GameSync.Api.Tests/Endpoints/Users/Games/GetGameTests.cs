using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.CommonResponses;
using GameSync.Api.Endpoints.Users.Me.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using Xunit;

namespace Tests.Endpoints.Users.Games;

[Collection("FullApp")]
public class GetGameTests : TestsWithLoggedUser
{
    public GetGameTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_existing_game_returns_it()
    {
        var expectedGame = await Factory.CreateTestGameAsync(UserId);

        var (response, result) = await Client.GETAsync<GetGame.Endpoint, RequestToIdentifiableObject, GameDetail>(new RequestToIdentifiableObject { Id = expectedGame.Id });

        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.Equivalent(result, expectedGame);
    }

    [Fact]
    public async Task Get_non_existing_game_produces_404()
    {
        var (response, _) = await Client.GETAsync<GetGame.Endpoint, RequestToIdentifiableObject, NotFound>(new RequestToIdentifiableObject { Id = 59484 });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}
