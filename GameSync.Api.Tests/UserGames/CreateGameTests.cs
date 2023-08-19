using Bogus.DataSets;
using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Endpoints.Users.Me.Collection;
using GameSync.Api.Persistence.Entities;
using IdentityModel.Client;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests.Collections;


[Collection("FullApp")]
public class CreateGameTests : IAsyncLifetime
{
    private readonly string mail = new Internet().Email();
    private const string password = "uPY994@euuK9&TPny#wSv5b";

    private readonly GameSyncAppFactory _factory;
    private readonly ITestOutputHelper _output;
    private HttpClient _client;   
    public CreateGameTests(GameSyncAppFactory factory, ITestOutputHelper output)
    {
        _client = factory.CreateClient();
        _factory = factory;
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

        var (response, result) = await _client.POSTAsync<CreateGameEndpoint, CreateGameRequest, BadRequestWhateverError>(newGameRequest);
        Assert.NotNull(result);
        Assert.Equal(5, result.Errors.Count());
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
        var (response, result) = await _client.POSTAsync<CreateGameEndpoint, CreateGameRequest, Game>(newGameRequest);

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

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        await _factory.CreateConfirmedUser(mail, mail, password);

        var (response, result) = await _client.POSTAsync<SignInEndpoint, SignInRequest, SuccessfulSignInResponse>(new SignInRequest { Email = mail, Password = password });
        _client.SetBearerToken(result.Token);
    }
}
