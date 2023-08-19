using GameSync.Api.Endpoints.Users.Me.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameSync.Api.Tests.EntityUser.Games;

public class GameValidatorTests
{
    private GameValidator _validator = new();

    [Fact]
    public void If_all_properties_are_correctly_set_no_errors_should_be_thrown()
    {
        var newGameRequest = new CreateGameRequest
        {
            MaxPlayer = 6,
            MinPlayer = 5,
            MinAge = 5,
            Name = "name",
            Description = "My description",
            DurationMinutes = 45
        };


        var result = _validator.Validate(newGameRequest);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Non_required_properties_should_be_ignored_if_null()
    {
        var newGameRequest = new CreateGameRequest
        {
            MaxPlayer = 6,
            MinPlayer = 5,
            MinAge = 5,
            Name = "name",
            Description = null,
            DurationMinutes = null
        };

        var result = _validator.Validate(newGameRequest);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }


    [Fact]
    public void MaxPlayer_must_be_greater_than_min_player()
    {
        var newGameRequest = new CreateGameRequest
        {
            MaxPlayer = 4,
            MinPlayer = 5,
            MinAge = 5,
            Name = "name"
        };

        var result = _validator.Validate(newGameRequest);

        Assert.False(result.IsValid);
        Assert.Equal(nameof(CreateGameRequest.MaxPlayer), Assert.Single(result.Errors).PropertyName);

    }

    [Fact]
    public void When_duration_is_negative_or_description_is_whitespace_validation_should_fail()
    {
        var newGameRequest = new CreateGameRequest
        {
            MaxPlayer = 6,
            MinPlayer = 5,
            MinAge = 5,
            Name = "name",
            Description = " ",
            DurationMinutes = -4
        };

        var result = _validator.Validate(newGameRequest);

        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
    }
}
