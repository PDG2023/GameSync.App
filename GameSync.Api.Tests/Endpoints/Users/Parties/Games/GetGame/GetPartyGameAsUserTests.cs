using Duende.IdentityServer.Validation;
using FastEndpoints;
using GameSync.Api.CommonResponses;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.PartyGame;
using GameSync.Api.Persistence.Entities;
using System.Net;
using Xunit;

namespace Tests.Endpoints.Users.Parties.Games.GetGame;

[Collection(GameSyncAppFactoryFixture.Name)]
public class GetPartyGameAsUserTests : TestsWithLoggedUser
{
    public GetPartyGameAsUserTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Non_existing_party_game_produces_404()
    {

        // arrange 
        var party = await Factory.CreateDefaultPartyAsync(UserId);
        var req = new GetPartyGame.Request { PartyGameId = 125012, PartyId = party.Id };

        // act
        var (response, _) = await Client.GETAsync<GetPartyGame.Endpoint, GetPartyGame.Request, GetPartyGame.Response>(req);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Get_party_game_of_other_user_produces_404()
    {
        // arrange
        var pg = await Factory.CreatePartyGameOfOtherUserAsync();
        var req = new GetPartyGame.Request { PartyGameId = pg.Id, PartyId = pg.PartyId };

        // act
        var (response, _) = await Client.GETAsync<GetPartyGame.Endpoint, GetPartyGame.Request, GetPartyGame.Response>(req);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }


    [Fact]
    public async Task Get_party_game_of_other_user_with_token_returns_the_game()
    {
        // arrange
        const string token = "other-user-token-12521";
        var pg = await Factory.CreatePartyGameOfOtherUserAsync(invitationToken: token);

        var req = new GetPartyGame.Request { PartyGameId = pg.Id, InvitationToken = token };

        // act
        var (response, res) = await Client.GETAsync<GetPartyGame.Endpoint, GetPartyGame.Request, GetPartyGame.Response>(req);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(res);
        Assert.Equal(pg.GameId, res.Game.Id);

    }

    [Fact]
    public async Task Get_party_game_of_self_returns_the_game()
    {
        // arrange
        var party = await Factory.CreateDefaultPartyAsync(UserId);
        var game = await Factory.CreateTestGameAsync(UserId);
        var pg = await Factory.CreatePartyGameAsync(party.Id, game.Id);
        var req = new GetPartyGame.Request { PartyGameId = pg.Id, PartyId = party.Id };

        // act
        var (response, res) = await Client.GETAsync<GetPartyGame.Endpoint, GetPartyGame.Request, GetPartyGame.Response>(req);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(res);
        Assert.Equal(pg.GameId, res.Game.Id);
    }




}
