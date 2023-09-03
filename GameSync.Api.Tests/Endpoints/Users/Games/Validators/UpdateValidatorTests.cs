using GameSync.Api.Endpoints.Users.Me.Games;
using Xunit;

namespace Tests.Endpoints.Users.Games.Validators;

public class UpdateValidatorTests
{

    private readonly UpdateGame.Validator _updateValidator = new();
    public static IEnumerable<object[]> MalformedUpdateRequest => new List<object[]>
    {
        new[] { new UpdateGame.Request { Id = 1, MinAge = -10 } },
        new[] { new UpdateGame.Request { Id = 1, Name = " " } },
        new[] { new UpdateGame.Request { Id = 1, MaxPlayer = -10 } },
        new[] { new UpdateGame.Request { Id = 1, MinPlayer = -10 } },
        new[] { new UpdateGame.Request { Id = 1, MaxPlayer = 10, MinPlayer = 11 } },
        new[] { new UpdateGame.Request { Id = 1, DurationMinute = -10 } }

    };

    [Fact]
    public void Ignorable_properties_should_not_produce_errors()
    {
        var request = new UpdateGame.Request { Id = 10 };

        var result = _updateValidator.Validate(request);
        Assert.Empty(result.Errors);

    }

    [Theory]
    [MemberData(nameof(MalformedUpdateRequest))]
    public void When_ignorable_properties_are_incorrect_they_should_produce_errors(UpdateGame.Request req)
    {
        var result = _updateValidator.Validate(req);
        Assert.Single(result.Errors);
    }
}