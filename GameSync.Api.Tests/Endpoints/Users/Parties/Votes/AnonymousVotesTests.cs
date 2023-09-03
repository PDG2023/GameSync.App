using FastEndpoints;
using GameSync.Api.Endpoints;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence.Migrations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace Tests.Endpoints.Users.Parties.Votes;

[Collection(GameSyncAppFactoryFixture.Name)]
public class AnonymousVotesTests
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;
    private const string _username = "Alfred";
    private const string _token = "token";

    public AnonymousVotesTests(GameSyncAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

    }

    [Fact]
    public async Task Anonymous_vote_without_token_produces_not_found()
    {
        // arrange
        var pg = await _factory.CreatePartyGameWithDependencyAsync();

        // act
        var (response, _) = await DoReq<Ok>(pg.PartyId, pg.GameId, _username, true);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_new_vote_creates_one()
    {
        // arrange
        var pg = await _factory.CreatePartyGameWithDependencyAsync(invitationToken: _token);

        // act
        var (response, _) = await DoReq<Ok>(pg.PartyId, pg.GameId, _username, true, _token);
        var vote = await GetVote(pg.PartyId, pg.GameId, _username);

        // assert
        response.EnsureSuccessStatusCode();

        Assert.NotNull(vote);
        Assert.True(vote.VoteYes);
        Assert.Equal(_username, vote.UserName);
    }

    [Fact]
    public async Task Put_false_in_existing_vote_updates_it_to_false()
    {
        // arrange
        var vote = new Vote { UserName = _username, VoteYes = true };
        var pg = await _factory.CreatePartyGameWithDependencyAsync(new List<Vote> { vote }, _token);

        // act
        var (response, _) = await DoReq<Ok>(pg.PartyId, pg.GameId, _username, false, _token);
        var voteResult = await GetVote(pg.PartyId, pg.GameId, _username);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(voteResult);
        Assert.False(voteResult.VoteYes);
        Assert.Equal(_username, vote.UserName);
    }

    [Fact]
    public async Task Put_new_vote_in_existing_list_adds_it()
    {
        // arrange
        const string otherUserName = "Other user";
        var vote = new Vote { UserName = otherUserName, VoteYes = true };
        var pg = await _factory.CreatePartyGameWithDependencyAsync(new List<Vote> { vote }, _token);

        // act
        var (response, _) = await DoReq<Ok>(pg.PartyId, pg.GameId, _username, false, _token);
        var voteResult = await GetVote(pg.PartyId, pg.GameId, _username);
        var otherUserVoteResult = await GetVote(pg.PartyId, pg.GameId, otherUserName);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(voteResult);
        Assert.False(voteResult.VoteYes);
        Assert.Equal(_username, voteResult.UserName);

        Assert.NotNull(otherUserVoteResult);
        Assert.True(otherUserVoteResult.VoteYes);
        Assert.Equal(otherUserName, otherUserVoteResult.UserName);

    }


    private async Task<Vote?> GetVote(int partyId, int gameId, string username)
    {
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var pg = await ctx.PartiesGames.AsNoTracking().FirstAsync(pg => pg.PartyId == partyId && pg.GameId == gameId);
        return pg
            .Votes?
            .FirstOrDefault(r => r.UserName == username);

    }

    private async Task<TestResult<TRes>> DoReq<TRes>(int partyId, int gameId, string username, bool? voteYes, string? token = null)
    {
        var voteReq = new GameVote.Request
        {
            GameId = gameId,
            PartyId = partyId,
            UserName = username,
            VoteYes = voteYes,
            InvitationToken = token
        };

        return await _client.PUTAsync<GameVote.Endpoint, GameVote.Request, TRes>(voteReq);

    }
}
