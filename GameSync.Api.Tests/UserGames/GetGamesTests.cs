using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence.Entities;
using GameSync.Business.BoardGameGeek.Model;
using Xunit;

namespace GameSync.Api.Tests.UserGames;

[Collection("FullApp")]
public class GetGamesTests : TestsWithLoggedUser
{
    public GetGamesTests(GameSyncAppFactory factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task User_without_games_should_return_an_empty_array()
    {
        var (response, result) = await Client.GETAsync<GetAllGames.Endpoint, IEnumerable<Game>>();

        Assert.NotNull(result);
        response.EnsureSuccessStatusCode();
        Assert.Empty(result);
    }

    [Fact]
    public async Task Getting_detail_of_game_should_work()
    {
        // arrange
        var games = await Task.WhenAll(
            Factory.CreateTestGame(UserId),
            Factory.CreateTestGame(UserId)

        );
        var req = new RequestToIdentifiableObject { Id = games[0].Id };

        // act
        var (response, result) = await Client.GETAsync<GetGame.Endpoint, RequestToIdentifiableObject, Game>(req);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.Equivalent(games[0], result);
    }

    [Fact]
    public async Task Getting_games_of_user_with_two_games_should_return_them_all()
    {
        // arrange : add said games
        var expectedFirstGame = await Factory.CreateTestGame(UserId);
        var expectedSecondGame = await Factory.CreateTestGame(UserId);

        // act 
        var (response, result) = await Client.GETAsync<GetAllGames.Endpoint, IEnumerable<GamePreview>>();

        // assert
        Assert.NotNull(result);
        response.EnsureSuccessStatusCode();
        Assert.Collection(result,
            first => AssertEquivalence(expectedFirstGame, first),
            second => AssertEquivalence(expectedSecondGame, second));
    }


    private void AssertEquivalence(Game expectedGame, GamePreview result)
    {
        var expected = new GamePreview
        {
            Id = expectedGame.Id,
            ImageUrl = expectedGame.ImageUrl,
            IsExpansion = expectedGame.IsExpansion,
            Name = expectedGame.Name,
            ThumbnailUrl = expectedGame.ThumbnailUrl,
            YearPublished = expectedGame.YearPublished
        };

        Assert.Equivalent(expected, result);
    }

}


