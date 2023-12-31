﻿using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Endpoints.Users.Parties;

[Collection(GameSyncAppFactoryFixture.Name)]
public class GetPartyAsAnonymousTests
{
    private readonly GameSyncAppFactory _factory;
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;

    public GetPartyAsAnonymousTests(GameSyncAppFactory factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task Getting_details_of_party_without_invitation_with_empty_token_in_requests_produces_not_found()
    {
        // arrange
        await _factory.CreatePartyOfAnotherUserAsync();

        // act
        var (response, _) = await DoReq(null);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Getting_details_of_party_with_invitation_with_inequal_token_produces_not_found()
    {
        // arrange
        await _factory.CreatePartyOfAnotherUserAsync("t1");

        // act
        var (response, _) = await DoReq("t2");

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Getting_details_of_party_with_correct_token_returns_it()
    {

        const string _token = "letsparty";

        // arrange
        await _factory.CreatePartyOfAnotherUserAsync(_token);

        // act
        var (response, res) = await DoReq(_token);

        // assert
        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
        Assert.NotNull(res);
        Assert.False(res.IsOwner);
    }

    private async Task<TestResult<GetParty.Response>> DoReq(string? invitationToken)
    {
        var req = new GetParty.Request { InvitationToken = invitationToken };
        return await _client.GETAsync<GetParty.Endpoint, GetParty.Request, GetParty.Response>(req);
    }
}

[Collection(GameSyncAppFactoryFixture.Name)]
public class GetPartyTests : TestsWithLoggedUser
{
    private readonly ITestOutputHelper _output;

    public GetPartyTests(GameSyncAppFactory factory, ITestOutputHelper output) : base(factory)
    {
        _output = output;
    }

    [Fact]
    public async Task Non_existing_party_produces_not_found()
    {
        var (response, _) = await DoReq<NotFound>(125125);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Getting_details_of_party_works()
    {
        // arrange
        var originalGame = await Factory.CreateTestGameAsync(UserId);
        var party = await Factory.CreatePartyAsync(new Party
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


        var pg = await Factory.CreatePartyGameAsync(party.Id, originalGame.Id, votes1);

        // act
        var (response, result) = await DoReq<GetParty.Response>(party.Id);

        // assert
        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
        Assert.NotNull(result);
        var expected = new
        {
            party.DateTime,
            party.Name,
            party.Location,
            result.IsOwner,
            GamesVoteInfo = new[]
            {
                new
                {
                    pg.Id,
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


    private async Task<TestResult<TRes>> DoReq<TRes>(int partyId)
    {
        var req = new GetParty.Request { Id = partyId };
        return await Client.GETAsync<GetParty.Endpoint, GetParty.Request, TRes>(req);
    }

}
