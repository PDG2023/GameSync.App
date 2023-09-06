using FastEndpoints;
using GameSync.Api.CommonResponses;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.PartyGame;
using System.Net;
using Xunit;

namespace Tests.Endpoints.Users.Parties.Games.GetGame;

[Collection(GameSyncAppFactoryFixture.Name)]
public class GetPartyGameAsAnonymousTests 
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;

    public GetPartyGameAsAnonymousTests(GameSyncAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async Task Get_game_of_unpublished_party_produces_404()
    {
        // arrange
        var pg = await _factory.CreatePartyGameOfOtherUserAsync();
        var req = new GetPartyGame.Request { PartyId = pg.PartyId, PartyGameId = pg.Id};

        // act
        var (response, _) = await _client.GETAsync<GetPartyGame.Endpoint, GetPartyGame.Request, GetPartyGame.Request>(req);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    

    [Fact]
    public async Task Get_game_with_valid_token_returns_it() 
    {
        // arrange
        var pg = await _factory.CreatePartyGameOfOtherUserAsync(invitationToken: "token");
        var req = new GetPartyGame.Request { PartyGameId = pg.Id, InvitationToken = "token"};

        // act
        var (response, res) = await _client.GETAsync<GetPartyGame.Endpoint, GetPartyGame.Request, GetPartyGame.Response>(req);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(res);
        Assert.Equal(pg.GameId,  res.Game.Id);

    }

}
