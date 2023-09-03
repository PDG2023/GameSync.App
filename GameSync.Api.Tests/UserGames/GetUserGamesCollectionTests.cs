using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Endpoints.Users.Me.Games.FromBgg;
using GameSync.Api.Persistence.Entities.Games;
using GameSync.Business.BoardGameGeek.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Org.BouncyCastle.Ocsp;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests.UserGames;

[Collection("FullApp")]
public class GetUserGamesCollectionTests : TestsWithLoggedUser
{
    private readonly ITestOutputHelper _output;

    public GetUserGamesCollectionTests(GameSyncAppFactory factory, ITestOutputHelper output) : base(factory)
    {
        _output = output;
    }

    [Fact]
    public async Task User_without_games_should_return_an_empty_array()
    {
        var (response, result) = await Client.GETAsync<GetAllGames.Endpoint, IEnumerable<CustomGame>>();

        Assert.NotNull(result);
        response.EnsureSuccessStatusCode();
        Assert.Empty(result);
    }

    [Fact]
    public async Task Getting_detail_of_custom_game_should_work()
    {
        // arrange
        var games = await Task.WhenAll(
            Factory.CreateTestGame(UserId),
            Factory.CreateTestGame(UserId)

        );
        var req = new RequestToIdentifiableObject { Id = games[0].Id };

        // act
        var (response, result) = await Client.GETAsync<GetGame.Endpoint, RequestToIdentifiableObject, CustomGame>(req);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.Equivalent(games[0], result);
    }

    [Fact]
    public async Task Getting_games_of_user_with_custom_and_bgg_game_returns_both()
    {
        // arrange 
        var customGame = await Factory.CreateTestGame(UserId);
        const int bggGameId = 8756;

        // act
        var (addGameResponse, _) = await Client.POSTAsync<AddBggGame.Endpoint, RequestToIdentifiableObject, Ok>(new RequestToIdentifiableObject { Id = bggGameId });
        var (getGamesResponse, result) = await Client.GETAsync<GetAllGames.Endpoint, IEnumerable<GameCollectionItem>>();

        // assert
        addGameResponse.EnsureSuccessStatusCode();
        await getGamesResponse.EnsureSuccessAndDumpBodyIfNotAsync(_output);

        var expectedCustomGame = new GameCollectionItem
        {
            Id = customGame.Id,
            ImageUrl = customGame.ImageUrl,
            IsExpansion = customGame.IsExpansion,
            Name = customGame.Name,
            ThumbnailUrl = customGame.ThumbnailUrl,
            YearPublished = customGame.YearPublished,
            IsCustom = true
            
        };

        var expectedBggGame = new GameCollectionItem
        {
            Id = bggGameId,
            IsCustom = false,
            ImageUrl= "https://cf.geekdo-images.com/CiA_AW2YaECs_ITtgkwCOA__original/img/HSfG89GifCM1kDbZshVswEmv-iA=/0x0/filters:format(jpeg)/pic51396.jpg",
            YearPublished = 1986,
            Name = "Eastern Front Solitaire",
            ThumbnailUrl = "https://cf.geekdo-images.com/CiA_AW2YaECs_ITtgkwCOA__thumb/img/TDCtncvTHAIKq2k-ZqonWgQ2WAg=/fit-in/200x150/filters:strip_icc()/pic51396.jpg",
            IsExpansion = false,
        };

        Assert.NotNull(result);
        Assert.Collection(
            result,
            resultCustomGame => Assert.Equivalent(expectedCustomGame, resultCustomGame),
            resultBggGame => Assert.Equivalent(expectedBggGame, resultBggGame));
    }

}


