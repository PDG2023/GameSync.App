using FastEndpoints;
using FastEndpoints.Security;
using GameSync.Api.Endpoints.Users;
using SQLitePCL;
using Xunit;

namespace GameSync.Api.Tests;

public class LogInTests : IClassFixture<GameSyncAppFactory>
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;

    private readonly string _mail = "ferati.kevin@gmail.com";
    private readonly string _password = "$UX#%A!qaphEL2";

    public LogInTests(GameSyncAppFactory integrationTestFactory)
    {
        _factory = integrationTestFactory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task LogInWithCorrectCredentials_ProduceValidJwtToken()
    {
        var loginRequest = new LogInRequest
        {
            Email = _mail,
            Password = _password
        };

        var (response, logInResult) = await _client.POSTAsync<LogInEndpoint, LogInRequest, LogInValidResponse>(loginRequest);
         
        response.EnsureSuccessStatusCode();

    }
}
