﻿using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Parties;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.Parties.Me;

[Collection("FullApp")]
public class UpdatePartyTests : TestsWithLoggedUser
{
    public UpdatePartyTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Updating_non_existing_party_produces_not_found()
    {
        // arrange
        var request = new UpdatePartyRequest { Id = 918582 };

        // act
        var (response, result) = await Client.PATCHAsync<UpdatePartyEndpoint, UpdatePartyRequest, NotFound>(request);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Updating_party_of_another_user_produces_not_found()
    {
        // arrange
        var partyId = await Factory.CreatePartyOfAnotherUser();
        var request = new UpdatePartyRequest 
        { 
            Id = partyId,
            DateTime = DateTime.Now.AddDays(1),
            Location = "...",
            Name = "."
        };

        // act
        var (response, result) = await Client.PATCHAsync<UpdatePartyEndpoint, UpdatePartyRequest, NotFound>(request);
        
        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Updating_party_of_user_changes_it()
    {
        // arrange
        var partyId = await Factory.CreateDefaultParty(UserId);
        var expectedDate = new DateTime(2025, 02, 04, 18, 0, 0);
        var request = new UpdatePartyRequest
        {
            Id = partyId,
            DateTime = expectedDate,
            Location = string.Empty, // removes the location
            Name = "Name"
        };

        // act
        var (response, result) = await Client.PATCHAsync<UpdatePartyEndpoint, UpdatePartyRequest, Party>(request);

        // assert
        response.EnsureSuccessStatusCode();

        Assert.NotNull(result);
        var expectedResults = new Party
        {
            Id = partyId,
            Name = request.Name,
            Location = request.Location,
            DateTime = expectedDate,
            UserId  = UserId,
            Games = null
        };

        Assert.Equivalent(expectedResults, result);
    }
}
