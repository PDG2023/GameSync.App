using Xunit;

namespace GameSync.Api.Tests.Identity;

[Collection("FullApp")]
public class ForgotPasswordTests 
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;
    public ForgotPasswordTests(GameSyncAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }


}
