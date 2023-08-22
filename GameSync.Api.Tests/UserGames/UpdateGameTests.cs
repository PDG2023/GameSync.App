using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.UserGames;

[Collection("FullApp")]
public class UpdateGameTests : TestsWithLoggedUser
{

    
    public UpdateGameTests(GameSyncAppFactory factory) : base(factory)
    {

    }


    [Fact]
    public async Task Trying_to_update_non_existing_game_produces_404()
    {
        // arrange
        var updateRequest = new UpdateGameRequest { Id = 10 };

        // act
        var (response, result) = await Client.PATCHAsync<UpdateGameEndpoint, UpdateGameRequest, NotFound>(updateRequest);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Malformed_properties_should_produce_errors()
    {
        // arrange
        var id = new Random().Next();
        var game = await Factory.CreateTestGame(UserId, id);

        var requestsToTests = GetMalformedRequests(game);

        foreach (var (request, property) in requestsToTests)
        {
            // act
            var (response, result) = await Client.PATCHAsync<UpdateGameEndpoint, UpdateGameRequest, BadRequestWhateverError>(request);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(property, Assert.Single(result.Errors).Property);
        }
    }

    [Fact]
    public async Task Html_is_escaped_and_all_inputs_are_taken()
    {
        // arrange
        var id = new Random().Next();
        await Factory.CreateTestGame(UserId, id);
        var request = new UpdateGameRequest
        {
            Id = id,
            Name = "<b>input</b>",
            Description = "<a>input</a>",
            DurationMinute = 10,
            MaxPlayer = 10,
            MinAge = 15,
            MinPlayer = 5
        };

        // act
        var (response, result) = await Client.PATCHAsync<UpdateGameEndpoint, UpdateGameRequest, Game>(request);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("&lt;b&gt;input&lt;/b&gt;", result.Name);
        Assert.Equal("&lt;a&gt;input&lt;/a&gt;", result.Description);
        Assert.Equal(request.DurationMinute, result.DurationMinute);
        Assert.Equal(request.MaxPlayer, result.MaxPlayer);
        Assert.Equal(request.MinAge, result.MinAge);
        Assert.Equal(request.MinPlayer, result.MinPlayer);

    }


    private static IEnumerable<(UpdateGameRequest req, string expectedErrorCode)> GetMalformedRequests(Game originalGame)
    {

        return new List<(UpdateGameRequest req, string expectedErrorCode)>
        {
            (
                new UpdateGameRequest { Id = originalGame.Id, MinPlayer = originalGame.MaxPlayer + 1 },
                nameof(Game.MinPlayer)
            ),

            (
                new UpdateGameRequest { Id = originalGame.Id, MaxPlayer = originalGame.MinPlayer - 1 },
                nameof(Game.MaxPlayer)
            ),


            (
                new UpdateGameRequest { Id = originalGame.Id, Description = string.Concat(Enumerable.Repeat('a', 1000)) },
                nameof(Game.Description)
            ),

            (
                new UpdateGameRequest { Id = originalGame.Id, DurationMinute = -10 },
                nameof(Game.DurationMinute)
            ),

            
            (
                new UpdateGameRequest { Id = originalGame.Id, MinAge = -10 },
                nameof(Game.MinAge)
            ),

            (
                new UpdateGameRequest { Id = originalGame.Id, Name = " " },
                nameof(Game.Name)
            )
        };
    }
}
