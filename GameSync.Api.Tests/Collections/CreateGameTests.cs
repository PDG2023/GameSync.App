using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Collection;
using Xunit;

namespace GameSync.Api.Tests.Collections;

[Collection("FullApp")]
public class CreateGameTests
{
    private HttpClient _client;   
    public CreateGameTests(GameSyncAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Creating_a_personal_game_should_return_the_same_game_and_create_it_in_the_storage()
    {
        // arrange
        var newGameRequest = new CreateGameRequest
        {
            MaxPlayer = 10,
            MinPlayer = 1,
            MinAge = 1,
            Name = "test-game"
        };

        // act
        var (response, result) = await _client.POSTAsync<CreateGameEndpoint, CreateGameRequest, CreateGameResponse>(newGameRequest);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.Equal(newGameRequest.Name, result.Name);
        Assert.Equal(newGameRequest.MinPlayer, result.MinPlayer);
        Assert.Equal(newGameRequest.MaxPlayer, result.MaxPlayer);
        Assert.Equal(newGameRequest.MinAge, result.MinAge);
    }

}
