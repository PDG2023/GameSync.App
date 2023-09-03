using GameSync.Api.Endpoints.Users.Me.Games;
using Xunit;

namespace Tests.Endpoints.Users.Games.Validators;

public class CreateValidatorTests
{

    private readonly CreateGame.Validator _validator = new();

    [Fact]
    public void If_all_properties_are_correctly_set_no_errors_should_be_thrown()
    {
        var newGameRequest = new CreateGame.Request
        {
            MaxPlayer = 6,
            MinPlayer = 5,
            MinAge = 5,
            Name = "name",
            Description = "My description",
            DurationMinute = 45
        };


        var result = _validator.Validate(newGameRequest);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Non_required_properties_should_be_ignored_if_null()
    {
        var newGameRequest = new CreateGame.Request
        {
            MaxPlayer = 6,
            MinPlayer = 5,
            MinAge = 5,
            Name = "name",
            Description = null,
            DurationMinute = null
        };

        var result = _validator.Validate(newGameRequest);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }


    [Fact]
    public void MaxPlayer_must_be_greater_than_min_player()
    {
        var newGameRequest = new CreateGame.Request
        {
            MaxPlayer = 4,
            MinPlayer = 5,
            MinAge = 5,
            Name = "name"
        };

        var result = _validator.Validate(newGameRequest);

        Assert.False(result.IsValid);
        Assert.Equal(nameof(CreateGame.Request.MaxPlayer), Assert.Single(result.Errors).PropertyName);

    }

    [Fact]
    public void When_duration_is_negative_validation_should_fail()
    {
        var newGameRequest = new CreateGame.Request
        {
            MaxPlayer = 6,
            MinPlayer = 5,
            MinAge = 5,
            Name = "name",
            Description = " ",
            DurationMinute = -4
        };

        var result = _validator.Validate(newGameRequest);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }
}
