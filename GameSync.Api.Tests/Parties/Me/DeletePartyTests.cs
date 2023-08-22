using Bogus.DataSets;
using Duende.IdentityServer.Validation;
using FastEndpoints;
using GameSync.Api.Common;
using GameSync.Api.Endpoints.Users.Me.Parties;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.Parties.Me;

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

        var (response, result) = await Client.DELETEAsync<DeletePartyEndpoint, RequestToIdentifiableObject, NotFound>(request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Deletion_of_party_of_another_user_produces_not_found()
    {
        // arrange : create another user and retrieve its id
        var otherUserId = await Factory.CreateConfirmedUser(new Internet().Email(), "username", "N#t$d6sM$F%#mgx9KanAtK!");
        var otherUserPartyId = await Factory.CreateParty(new Party
        {
            DateTimeOfParty = DateTime.Now,
            Location = "...",
            Name = "...",
            UserId = otherUserId
        });

        var request = new RequestToIdentifiableObject { Id = otherUserPartyId };

        // act
        var (response, result) = await Client.DELETEAsync<DeletePartyEndpoint, RequestToIdentifiableObject, NotFound>(request);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Deletion_of_existing_party_deletes_it_from_storage()
    {
        // arrange
        var partyId = await Factory.CreateParty(new Party
        {
            DateTimeOfParty = DateTime.Now,
            Location = "Somewhere",
            Name = "A name",
            UserId = UserId
        });

        var request = new RequestToIdentifiableObject { Id = partyId };

        //act
        var (response, result) = await Client.DELETEAsync<DeletePartyEndpoint, RequestToIdentifiableObject, Ok>(request);

        // assert
        response.EnsureSuccessStatusCode();

        // ensure the party is not there
        using var scope = Factory.Services.CreateScope();
        var ctx = scope.Resolve<GameSyncContext>();
        Assert.False(ctx.Parties.Any(x => x.Id == partyId));
    }

}
