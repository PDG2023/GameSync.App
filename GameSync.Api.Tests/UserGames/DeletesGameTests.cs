using FastEndpoints;
using GameSync.Api.Common;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.UserGames;

[Collection("FullApp")]
public class DeletesGameTests : TestsWithLoggedUser
{
    public DeletesGameTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Trying_to_delete_nonexisting_games_of_a_user_returns_not_found_without_deleting_the_found_games()
    {
        // arrange

        var games = await Task.WhenAll(
            Factory.CreateTestGame(UserId, 800),
            Factory.CreateTestGame(UserId, 801),
            Factory.CreateTestGame(UserId, 802)
        );

        var request = new SingleGameRequest { Id = 8080 };

        // act
        var (response, result) = await Client.DELETEAsync<DeleteGameEndpoint, SingleGameRequest, NotFound>(request);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        // checks whether the first two games are still there
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var remainingGames = await ctx.Games.Where(x => x.UserId == UserId).Select(x => x.Id).ToListAsync();
        Assert.True(games.Select(x => x.Id).All(remainingGames.Contains));
    }


    [Fact]
    public async Task Deleting_a_game_deletes_them_in_the_storage()
    {
        // arrange

        var games = await Task.WhenAll(
            Factory.CreateTestGame(UserId, 1000),
            Factory.CreateTestGame(UserId, 1001),
            Factory.CreateTestGame(UserId, 1002)
        );

        var request = new SingleGameRequest { Id = 1000 };

        // act
        var (response, result) = await Client.DELETEAsync<DeleteGameEndpoint, SingleGameRequest, Ok>(request);

        // assert
        response.EnsureSuccessStatusCode();

        // Checks whether the deleted game is in fact correctly deleted
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var gamesShouldBeDeleted = await ctx.Games.Where(x => x.Id == 1000).ToListAsync();
        Assert.Empty(gamesShouldBeDeleted);
    }

}
