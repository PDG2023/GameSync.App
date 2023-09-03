using FastEndpoints;
using GameSync.Api;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence.Entities.Games;
using Tests;
using Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Endpoints.Users.Games;


[Collection(GameSyncAppFactoryFixture.Name)]
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

        var newGameRequest = new CreateGame.Request
        {
            MaxPlayer = -10,
            MinPlayer = -3,
            MinAge = -2,
            Name = string.Empty,
            Description = string.Empty,
            DurationMinute = -5
        };

        var (response, result) = await Client.POSTAsync<CreateGame.Endpoint, CreateGame.Request, BadRequestWhateverError>(newGameRequest);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(7, result.Errors.Count());
    }

    [Fact]
    public async Task Creating_a_personal_game_should_return_the_same_game_with_escaped_html()
    {
        // arrange
        var newGameRequest = new CreateGame.Request
        {
            MaxPlayer = 10,
            MinPlayer = 1,
            MinAge = 1,
            Name = "<b>test-game</b>",
            Description = "<b>description</b>",
            DurationMinute = 10
        };

        // act
        var (response, result) = await Client.POSTAsync<CreateGame.Endpoint, CreateGame.Request, CustomGame>(newGameRequest);

        // assert
        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);

        Assert.NotNull(result);
        Assert.Equal("&lt;b&gt;test-game&lt;/b&gt;", result.Name);
        Assert.Equal("&lt;b&gt;description&lt;/b&gt;", result.Description);
        Assert.Equal(newGameRequest.DurationMinute, result.DurationMinute);
        Assert.Equal(newGameRequest.MinPlayer, result.MinPlayer);
        Assert.Equal(newGameRequest.MaxPlayer, result.MaxPlayer);
        Assert.Equal(newGameRequest.MinAge, result.MinAge);
    }

}
