using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Endpoints.Users.Me.Games.FromBgg;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace Tests.Endpoints.Users.Games;

[Collection(GameSyncAppFactoryFixture.Name)]
public class DeletesGameTests : TestsWithLoggedUser
{
    public DeletesGameTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Deleting_a_non_existing_game_produces_not_found()
    {
        // arrange
        var request = new DeleteGame.Request { Id = 8080, IsCustomGame = false };
        var requestCustom = new DeleteGame.Request { Id = 8080, IsCustomGame = true };

        // act
        var (response, _) = await Client.DELETEAsync<DeleteGame.Endpoint, DeleteGame.Request, NotFound>(request);
        var (responseCustom, _) = await Client.DELETEAsync<DeleteGame.Endpoint, DeleteGame.Request, NotFound>(requestCustom);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, responseCustom.StatusCode);
    }


    [Fact]
    public async Task Deleting_a_game_deletes_them_in_the_storage()
    {
        // arrange

        var games = await Task.WhenAll(
            Factory.CreateTestGameAsync(UserId),
            Factory.CreateTestGameAsync(UserId),
            Factory.CreateTestGameAsync(UserId)
        );
        var needleId = games[0].Id;
        var request = new DeleteGame.Request { Id = needleId, IsCustomGame = true };

        // act
        var (response, result) = await Client.DELETEAsync<DeleteGame.Endpoint, DeleteGame.Request, Ok>(request);

        // assert
        response.EnsureSuccessStatusCode();

        // Checks whether the deleted game is in fact correctly deleted
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var gamesShouldBeDeleted = await ctx.CustomGames.Where(x => x.Id == needleId).ToListAsync();
        Assert.Empty(gamesShouldBeDeleted);
    }


    [Fact]
    public async Task Deleting_a_link_between_a_user_and_a_bgg_game_works()
    {
        // arrange
        const int gameId = 321016;
        var request = new DeleteGame.Request { Id = gameId, IsCustomGame = false };

        // act
        var (responseAdd, _) = await Client.POSTAsync<AddBggGame.Endpoint, RequestToIdentifiableObject, Ok>(request);
        var (response, result) = await Client.DELETEAsync<DeleteGame.Endpoint, DeleteGame.Request, Ok>(request);

        // assert
        responseAdd.EnsureSuccessStatusCode();
        response.EnsureSuccessStatusCode();

        // Checks whether the deleted game is in fact correctly deleted
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var gameExists = await ctx.UserBoardGameGeekGames.AnyAsync(x => x.BoardGameGeekGameId == gameId && x.UserId == UserId);
        Assert.False(gameExists);
    }

}
