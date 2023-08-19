using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence.Entities;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests.UserGames;


[Collection("FullApp")]
public class CreateGameTests : TestsWithLoggedUser
{

    private readonly ITestOutputHelper _output;
    public CreateGameTests(GameSyncAppFactory factory, ITestOutputHelper output) : base(factory)
    {
        _output = output;
    }

    [Fact]
    public async Task Invalid_properties_should_produce_errors()
    {

        var newGameRequest = new CreateGameRequest
        {
            MaxPlayer = -10,
            MinPlayer = -3,
            MinAge = -2,
            Name = string.Empty,
            Description = string.Empty,
            DurationMinutes = -5
        };

        var (response, result) = await Client.POSTAsync<CreateGameEndpoint, CreateGameRequest, BadRequestWhateverError>(newGameRequest);
        Assert.NotNull(result);
        Assert.Equal(7, result.Errors.Count());
    }

    [Fact]
    public async Task Creating_a_personal_game_should_return_the_same_game_with_escaped_html()
    {
        // arrange
        var newGameRequest = new CreateGameRequest
        {
            MaxPlayer = 10,
            MinPlayer = 1,
            MinAge = 1,
            Name = "<b>test-game</b>",
            Description = "<b>description</b>",
            DurationMinutes = 10
        };

        // act
        var (response, result) = await Client.POSTAsync<CreateGameEndpoint, CreateGameRequest, Game>(newGameRequest);

        // assert
        await response.EnsureSuccessAndDumpBodyIfNot(_output);

        Assert.NotNull(result);
        Assert.Equal("&lt;b&gt;test-game&lt;/b&gt;", result.Name);
        Assert.Equal("&lt;b&gt;description&lt;/b&gt;", result.Description);
        Assert.Equal(newGameRequest.DurationMinutes, result.DurationMinute);
        Assert.Equal(newGameRequest.MinPlayer, result.MinPlayer);
        Assert.Equal(newGameRequest.MaxPlayer, result.MaxPlayer);
        Assert.Equal(newGameRequest.MinAge, result.MinAge);
    }

}
