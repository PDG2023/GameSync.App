using Bogus;
using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;
using Xunit;

namespace GameSync.Api.Tests.Parties.Games;

[Collection("FullApp")]
public class GetPartyDetailsTests : TestsWithLoggedUser
{
    public GetPartyDetailsTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Non_existing_party_produces_not_found()
    {
        var (response, _) = await DoReq<NotFound>(125125);

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Getting_details_of_party_works()
    {
        // arrange
        var originalGame = await Factory.CreateTestGame(UserId);
        var party = await Factory.CreateParty(new Party
        {
            DateTime = new DateTime(2025, 3, 3, 19, 0, 0),
            Name = "Party",
            Location = "Test Location",
            UserId = UserId,
        });

        var votes1 = new List<Vote>
        {
            new Vote
            {
                UserId = UserId,
                VoteYes = true,
            },

            new Vote
            {
                UserName = "a1",
                VoteYes = true,
            },

            new Vote
            {
                VoteYes = false,
                UserName = "a2"
            }
        };
        

        await Factory.CreatePartyGame(party.Id, originalGame.Id, votes1);

        // act
        var (response, result) = await DoReq<GetPartyDetails.Response>(party.Id);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        var expected = new
        {
            party.DateTime,
            party.Name,
            party.Location,
            GamesVoteInfo = new[]
            {
                new
                {
                    GameImageUrl = originalGame.ImageUrl,
                    GameName = originalGame.Name,
                    WhoVotedNo = new [] { "a2" },
                    WhoVotedYes = new[] {Mail, "a1"},
                    CountVotedYes = 2,
                    CountVotedNo = 1
                }
            },
        };

        Assert.Equivalent(expected, result);

    }


    private async Task<TestResult<TRes>>  DoReq<TRes>(int partyId)
    {
        var req = new RequestToIdentifiableObject { Id = partyId };
        return await Client.GETAsync<GetPartyDetails.Endpoint, RequestToIdentifiableObject, TRes>(req);
    }

}
