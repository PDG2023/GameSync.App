using FastEndpoints;
using GameSync.Api;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.PartyGame;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Runtime.CompilerServices;
using Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Endpoints.Users.Parties.Games;

[Collection(GameSyncAppFactoryFixture.Name)]
public class AddGameToPartyTests : TestsWithLoggedUser
{
    private readonly ITestOutputHelper _output;

    public AddGameToPartyTests(GameSyncAppFactory factory, ITestOutputHelper output) : base(factory)
    {
        _output = output;
    }



    [Fact]
    public async Task Add_non_existing_game_of_user_collection_produces_not_found()
    {
        // arrange
        var party = await Factory.CreateDefaultPartyAsync(UserId);
        var request = new AddPartyGames.Request 
        { 
            PartyId = party.Id, 
            Games = new[] 
            {
                new AddPartyGames.Request.PartyGameInfo()
                {
                    Id = 4554854,
                    IsCustom = true,
                }
            }
        };

        // act

        var (response, _)
            = await Client.POSTAsync<AddPartyGames.Endpoint, AddPartyGames.Request, NotFound>(request);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Add_existing_game_to_party_stores_it()
    {
        // arrange
        var request = await GetRequestToPartyGame(UserId);

        // act
        var (response, _)
            = await Client.POSTAsync<AddPartyGames.Endpoint, AddPartyGames.Request, Ok>(request);

        // assert
        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
    }

    [Fact]
    public async Task Adding_twice_same_game_in_party_produces_bad_request()
    {
        // arrange
        var request = await GetRequestToPartyGame(UserId);

        // act
        await Client.POSTAsync<AddPartyGames.Endpoint, AddPartyGames.Request, Ok>(request);
        var (response, _) = await Client.POSTAsync<AddPartyGames.Endpoint, AddPartyGames.Request, Ok>(request);

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }

    public async Task<AddPartyGames.Request> GetRequestToPartyGame(string userId)
    {
        var party = await Factory.CreateDefaultPartyAsync(userId);
        var game = await Factory.CreateTestGameAsync(userId);
        return new AddPartyGames.Request
        {
            Games = new[]
            {
                new AddPartyGames.Request.PartyGameInfo
                {
                    Id = game.Id,
                    IsCustom = true,
                }
            },
            PartyId = party.Id
        };
    }
}
