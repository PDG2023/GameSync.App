using Xunit;

namespace GameSync.Api.Tests.Parties.Me.Games;

[Collection("FullApp")]
public class AddGameToPartyTests : TestsWithLoggedUser
{
    public AddGameToPartyTests(GameSyncAppFactory factory) : base(factory)
    {
    }

}
