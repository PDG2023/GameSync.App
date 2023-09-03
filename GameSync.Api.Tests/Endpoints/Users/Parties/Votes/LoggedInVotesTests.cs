using FastEndpoints;
using GameSync.Api.Endpoints;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests;

namespace Tests.Endpoints.Users.Parties.Votes;

[Collection(GameSyncAppFactoryFixture.Name)]
public class LoggedInVotesTests : TestsWithLoggedUser
{
    public LoggedInVotesTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Put_new_vote_creates_it_with_user_id()
    {
        // arrange
        var pg = await Factory.CreatePartyGameWithDependencyAsync();

        // act
        var (response, _) = await DoReq<Ok>(pg.PartyId, pg.GameId, true);
        var vote = await GetVote(pg.PartyId, pg.GameId);

        // assert
        response.EnsureSuccessStatusCode();

        Assert.NotNull(vote);
        Assert.True(vote.VoteYes);
        Assert.Equal(UserId, vote.UserId);
    }


    [Fact]
    public async Task Put_false_in_existing_vote_updates_it_to_false()
    {
        // arrange
        var vote = new Vote { UserId = UserId, VoteYes = true };
        var pg = await Factory.CreatePartyGameWithDependencyAsync(new List<Vote> { vote });

        // act
        var (response, _) = await DoReq<Ok>(pg.PartyId, pg.GameId, false); ;
        var voteResult = await GetVote(pg.PartyId, pg.GameId);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(voteResult);
        Assert.False(voteResult.VoteYes);
        Assert.Equal(UserId, vote.UserId);
    }


    [Fact]
    public async Task Put_new_vote_in_existing_list_adds_it()
    {
        // arrange
        const string otherUsername = "Hello";
        var vote = new Vote { UserName = otherUsername, VoteYes = true };
        var existingPartyGame = await Factory.CreatePartyGameWithDependencyAsync(new List<Vote> { vote });

        // act
        var (response, _) = await DoReq<Ok>(existingPartyGame.PartyId, existingPartyGame.GameId, false);
        var voteResult = await GetVote(existingPartyGame.PartyId, existingPartyGame.GameId);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(voteResult);
        Assert.False(voteResult.VoteYes);
        Assert.Equal(UserId, voteResult.UserId);

        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var insertedPg = await ctx.PartiesGames
            .AsNoTracking()
            .FirstAsync(pg => pg.PartyId == existingPartyGame.PartyId && pg.GameId == existingPartyGame.GameId);
        var otherUserVote = insertedPg.Votes!.FirstOrDefault(pg => pg.UserName == otherUsername);
        Assert.NotNull(otherUserVote);
    }


    private async Task<TestResult<TRes>> DoReq<TRes>(int partyId, int gameId, bool? voteYes)
    {
        var voteReq = new GameVote.Request
        {
            GameId = gameId,
            PartyId = partyId,
            VoteYes = voteYes
        };

        return await Client.PUTAsync<GameVote.Endpoint, GameVote.Request, TRes>(voteReq);

    }

    public async Task<Vote?> GetVote(int partyId, int gameId)
    {
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var pg = await ctx.PartiesGames.AsNoTracking().FirstAsync(pg => pg.PartyId == partyId && pg.GameId == gameId);
        return pg
            .Votes?
            .FirstOrDefault(r => r.UserId == UserId);

    }

}
