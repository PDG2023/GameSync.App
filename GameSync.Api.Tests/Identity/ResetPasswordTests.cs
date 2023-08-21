using Xunit;

namespace GameSync.Api.Tests.Identity;

[Collection("FullApp")]
public class ResetPasswordTests : TestsWithLoggedUser
{
    public ResetPasswordTests(GameSyncAppFactory factory) : base(factory)
    {
    }

}
