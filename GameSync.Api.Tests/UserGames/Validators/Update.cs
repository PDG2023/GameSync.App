﻿using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using GameSync.Api.Endpoints.GameSearch;
using GameSync.Api.Endpoints.Users.Me.Games;
using Xunit;

namespace GameSync.Api.Tests.UserGames.Validators;

public class Update
{

    private readonly UpdateGameValidator _updateValidator = new();
    public static IEnumerable<object[]> MalformedUpdateRequest => new List<object[]>
    {
        new[] { new UpdateGameRequest { GameId = 1, MinAge = -10 } },
        new[] { new UpdateGameRequest { GameId = 1, Description = " " } },
        new[] { new UpdateGameRequest { GameId = 1, Name = " " } },
        new[] { new UpdateGameRequest { GameId = 1, MaxPlayer = -10 } },
        new[] { new UpdateGameRequest { GameId = 1, MinPlayer = -10 } },
        new[] { new UpdateGameRequest { GameId = 1, MaxPlayer = 10, MinPlayer = 11 } },
        new[] { new UpdateGameRequest { GameId = 1, DurationMinutes = -10 } }

    };

    [Fact]
    public void Ignorable_properties_should_not_produce_errors()
    {
        var updateGameRequest = new UpdateGameRequest { GameId = 10 };

        var result = _updateValidator.Validate(updateGameRequest);
        Assert.Empty(result.Errors);

    }

    [Theory]
    [MemberData(nameof(MalformedUpdateRequest))]
    public void When_ignorable_properties_are_incorrect_they_should_produce_errors(UpdateGameRequest req)
    {
        var result = _updateValidator.Validate(req);
        Assert.Single(result.Errors);
    }
}