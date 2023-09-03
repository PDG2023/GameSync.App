using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Parties;
using GameSync.Api.Persistence.Entities;
using Tests;
using Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Endpoints.Users.Parties;

[Collection("FullApp")]
public class GetAllPartiesTests : TestsWithLoggedUser
{
    private readonly ITestOutputHelper _output;

    public GetAllPartiesTests(GameSyncAppFactory factory, ITestOutputHelper output) : base(factory)
    {
        _output = output;
    }

    [Fact]
    public async Task Retrieve_existing_parties_of_user_returns_them_with_the_count_of_games()
    {
        // arrange

        var date = DateTime.Now.AddDays(1);

        var game = await Factory.CreateTestGameAsync(UserId);
        var otherGame = await Factory.CreateTestGameAsync(UserId);

        var expectedFirstParty = new Party
        {
            DateTime = date,
            Name = "First Party",
            UserId = UserId,
            Games = null,
            Location = "First Party Location"
        };

        var expectedSecondParty = new Party
        {
            DateTime = date,
            Name = "Second Party",
            UserId = UserId,
            Location = "Second Party Location",
            Games = null
        };

        var parties = await Task.WhenAll(
            Factory.CreatePartyAsync(expectedFirstParty),
            Factory.CreatePartyAsync(expectedSecondParty)
        );

        await Factory.CreatePartyGameAsync(parties[1].Id, game.Id);
        await Factory.CreatePartyGameAsync(parties[1].Id, otherGame.Id);


        // act
        var (response, result) = await Client.GETAsync<GetAllParties.Endpoint, IEnumerable<PartyPreview>>();

        // assert
        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
        Assert.NotNull(result);

        Assert.Collection(
            result,
            firstParty => AssertEquivalence(expectedFirstParty, firstParty, 0),
            secondParty => AssertEquivalence(expectedSecondParty, secondParty, 2)
        );

        void AssertEquivalence(Party expected, PartyPreview result, int numberOfGames)
        {
            var expectedProperties = new
            {
                expected.Name,
                expected.Location,
                expected.Id,
                NumberOfGames = numberOfGames
            };
            Assert.Equivalent(expectedProperties, result);
            CompareDates(expected.DateTime, result.DateTime);
        }

        void CompareDates(DateTime expected, DateTime actual)
        {
            var expectedTruncated = new DateTime(expected.Year, expected.Month, expected.Day, expected.Hour, expected.Minute, expected.Second);
            var actualTruncated = new DateTime(expected.Year, expected.Month, expected.Day, expected.Hour, expected.Minute, expected.Second);

            Assert.Equal(expectedTruncated, actualTruncated);
        }
    }
}
