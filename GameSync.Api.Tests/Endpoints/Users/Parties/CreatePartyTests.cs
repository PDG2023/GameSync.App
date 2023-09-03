using FastEndpoints;
using GameSync.Api;
using GameSync.Api.Endpoints.Users.Me.Parties;
using System.Net;
using Xunit;

namespace Tests.Endpoints.Users.Parties;

[Collection(GameSyncAppFactoryFixture.Name)]
public class CreatePartyTests : TestsWithLoggedUser
{
    public CreatePartyTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Creating_a_party_with_valid_properties_returns_it_with_encoded_html()
    {
        // arrange
        var date = new DateTime(2025, 08, 08, 17, 0, 0);
        var request = new CreateParty.Request
        {
            DateTime = date,
            Name = "<script>hackerman</script>",
            Location = "<b>trying some stuff</b>"
        };


        // act
        var (response, result) = await Client.POSTAsync<CreateParty.Endpoint, CreateParty.Request, PartyPreview>(request);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        var expected = new
        {
            Location = "&lt;b&gt;trying some stuff&lt;/b&gt;",
            Name = "&lt;script&gt;hackerman&lt;/script&gt;",
            NumberOfGames = 0,
            DateTime = date
        };
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public async Task Creating_a_party_with_invalid_properties_produces_not_found()
    {
        // arrange
        var request = new CreateParty.Request
        {
            DateTime = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
            Name = string.Empty
        };

        // act
        var (response, result) = await Client.POSTAsync<CreateParty.Endpoint, CreateParty.Request, BadRequestWhateverError>(request);

        // assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(2, result.Errors.Count());

    }
}
