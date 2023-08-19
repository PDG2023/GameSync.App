using GameSync.Api.Endpoints.Users.Me.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GameSync.Api.Tests.UserGames;

public class CreateOrUpdateGameValidatorTests
{
    private CreateOrUpdateGameValidator _validator = new();

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
            DurationMinute = null
        };

        var result = _validator.Validate(newGameRequest);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        

    }

    [Fact]
    public void When_description_and_duration_are_negative_validation_should_fail()
    {
       
    }
}
