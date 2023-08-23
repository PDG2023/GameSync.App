using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Parties;
using GameSync.Api.Persistence.Entities;
using Xunit;

namespace GameSync.Api.Tests.Parties.Me;

[Collection("FullApp")]
public class GetAllPartiesTests : TestsWithLoggedUser
{
    public GetAllPartiesTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Retrieve_existing_parties_of_user_returns_them_with_the_count_of_games()
    {
        // arrange
        var date = DateTime.Now.AddDays(1);
        var parties = await Task.WhenAll(
            Factory.CreateParty(new Party
            {
                DateTime = date,
                Name = "First Party",
                UserId = UserId,
                Games = null,
                Location = "First Party Location"
            }),

            Factory.CreateParty(new Party 
            {
                DateTime = date,
                Name = "Second Party",
                UserId = UserId,
                Location = "Second Party Location",
                Games = new[] { 
                    new PartyGame { GameId = 0 }, 
                    new PartyGame { GameId = 1 } 
                }
            })
        );


        // act
        var (response, result) = await Client.GETAsync<GetAllParties.Endpoint, IEnumerable<PartyPreview>>();
        
        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);

        Assert.Collection(
            result, 

            firstParty =>
            {
                var expected = new
                {
                    Id = parties[0],
                    Name = "First Party",
                    NumberOfGames = 0,
                    Location = "First Party Location"
                };
                Assert.Equivalent(expected, firstParty);
                CompareDates(date, firstParty.DateTime);
            },

            secondParty =>
            {
                var expected = new
                {
                    Id = parties[1],
                    Name = "Second Party",
                    NumberOfGames = 2,
                    Location = "Second Party Location"
                };
                Assert.Equivalent(expected, secondParty);
                CompareDates(date, secondParty.DateTime);
            }
        );

        void CompareDates(DateTime expected, DateTime actual)
        {
            var expectedTruncated = new DateTime(expected.Year, expected.Month, expected.Day, expected.Hour, expected.Minute, expected.Second);
            var actualTruncated = new DateTime(expected.Year, expected.Month, expected.Day, expected.Hour, expected.Minute, expected.Second);

            Assert.Equal(expectedTruncated, actualTruncated);
        }
    }
}
