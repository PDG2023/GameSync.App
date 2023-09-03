using FastEndpoints;
using GameSync.Api;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using Tests;
using Xunit;

namespace Tests.Endpoints.Users.Games;

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
        var updateRequest = new UpdateGame.Request { Id = 10 };

        // act
        var (response, _) = await Client.PATCHAsync<UpdateGame.Endpoint, UpdateGame.Request, NotFound>(updateRequest);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Malformed_properties_should_produce_errors()
    {
        // arrange
        var game = await Factory.CreateTestGame(UserId);

        var requestsToTests = GetMalformedRequests(game);

        foreach (var (request, property) in requestsToTests)
        {
            // act
            var (response, result) = await Client.PATCHAsync<UpdateGame.Endpoint, UpdateGame.Request, BadRequestWhateverError>(request);

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
        var game = await Factory.CreateTestGame(UserId);
        var request = new UpdateGame.Request
        {
            Id = game.Id,
            Name = "<b>input</b>",
            Description = "<a>input</a>",
            DurationMinute = 10,
            MaxPlayer = 10,
            MinAge = 15,
            MinPlayer = 5
        };

        // act
        var (response, result) = await Client.PATCHAsync<UpdateGame.Endpoint, UpdateGame.Request, CustomGame>(request);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.Equal("&lt;b&gt;input&lt;/b&gt;", result.Name);
        Assert.Equal("&lt;a&gt;input&lt;/a&gt;", result.Description);
        Assert.Equal(request.DurationMinute, result.DurationMinute);
        Assert.Equal(request.MaxPlayer, result.MaxPlayer);
        Assert.Equal(request.MinAge, result.MinAge);
        Assert.Equal(request.MinPlayer, result.MinPlayer);

    }


    private static IEnumerable<(UpdateGame.Request req, string expectedErrorCode)> GetMalformedRequests(CustomGame originalGame)
    {

        return new List<(UpdateGame.Request req, string expectedErrorCode)>
        {
            (
                new UpdateGame.Request { Id = originalGame.Id, MinPlayer = originalGame.MaxPlayer + 1 },
                nameof(CustomGame.MinPlayer)
            ),

            (
                new UpdateGame.Request { Id = originalGame.Id, MaxPlayer = originalGame.MinPlayer - 1 },
                nameof(CustomGame.MaxPlayer)
            ),


            (
                new UpdateGame.Request { Id = originalGame.Id, Description = string.Concat(Enumerable.Repeat('a', 1000)) },
                nameof(CustomGame.Description)
            ),

            (
                new UpdateGame.Request { Id = originalGame.Id, DurationMinute = -10 },
                nameof(CustomGame.DurationMinute)
            ),


            (
                new UpdateGame.Request { Id = originalGame.Id, MinAge = -10 },
                nameof(CustomGame.MinAge)
            ),

            (
                new UpdateGame.Request { Id = originalGame.Id, Name = " " },
                nameof(CustomGame.Name)
            )
        };
    }
}
