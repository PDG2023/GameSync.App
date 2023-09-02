using Bogus;
using Bogus.DataSets;
using Duende.IdentityServer.Validation;
using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.Parties;

[Collection("FullApp")]
public class DeletePartyTests : TestsWithLoggedUser
{
    public DeletePartyTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Deletion_of_non_existing_party_produces_not_found()
    {
        var request = new RequestToIdentifiableObject { Id = 295203 };

        var (response, _) = await Client.DELETEAsync<DeleteParty.Endpoint, RequestToIdentifiableObject, NotFound>(request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Deletion_of_party_of_another_user_produces_not_found()
    {
        // arrange : create another user and retrieve its id
        var otherUserParty = await Factory.CreatePartyOfAnotherUser();

        var request = new RequestToIdentifiableObject { Id = otherUserParty.Id };

        // act
        var (response, _) = await Client.DELETEAsync<DeleteParty.Endpoint, RequestToIdentifiableObject, NotFound>(request);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Deletion_of_existing_party_deletes_it_from_storage()
    {
        // arrange
        var party = await Factory.CreateDefaultParty(UserId);
        var request = new RequestToIdentifiableObject { Id = party.Id };

        //act
        var (response, _) = await Client.DELETEAsync<DeleteParty.Endpoint, RequestToIdentifiableObject, Ok>(request);

        // assert
        response.EnsureSuccessStatusCode();

        // ensure the party is not there
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        Assert.False(ctx.Parties.Any(x => x.Id == party.Id));
    }

}
