using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Endpoints.Users.Parties;

[Collection("FullApp")]
public class GetInvitationLinkTests : TestsWithLoggedUser
{
    private readonly ITestOutputHelper _output;

    public GetInvitationLinkTests(GameSyncAppFactory factory, ITestOutputHelper output) : base(factory)
    {
        _output = output;
    }

    [Fact]
    public async Task Retrieving_invitation_link_of_party_without_one_creates_it()
    {
        // arrange
        var party = await Factory.CreateDefaultPartyAsync(UserId);

        // act
        var (response, result) = await DoReq(party.Id);

        // assert
        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
        Assert.NotNull(result);

        using var scope = Factory.Services.CreateAsyncScope();
        var ctx = scope.Resolve<GameSyncContext>();
        var retrievedParty = await ctx.Parties.FirstAsync(p => p.Id == party.Id);
        Assert.NotNull(retrievedParty.InvitationToken);

    }

    [Fact]
    public async Task Retrieving_invitation_link_of_party_with_one_does_not_change_it()
    {

        // arrange
        const string expectedToken = "expected";
        var party = await Factory.CreatePartyAsync(new Party
        {
            DateTime = DateTime.Now.AddDays(2),
            Name = "...",
            InvitationToken = expectedToken,
            UserId = UserId
        });

        // act
        var (response, result) = await DoReq(party.Id);

        // assert
        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
        Assert.NotNull(result);
        Assert.EndsWith(expectedToken, result);
    }

    private async Task<TestResult<string>> DoReq(int partyId)
    {
        var req = new RequestToIdentifiableObject { Id = partyId };
        return await Client.GETAsync<GetInvitationLink.Endpoint, RequestToIdentifiableObject, string>(req);
    }

}
