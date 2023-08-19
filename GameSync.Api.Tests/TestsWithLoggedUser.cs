using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using IdentityModel.Client;
using Xunit;

namespace GameSync.Api.Tests;

public class TestsWithLoggedUser : IAsyncLifetime
{

    protected string Mail { get; } = new Bogus.DataSets.Internet().Email();
    protected string Password { get; } = "uPY994@euuK9&TPny#wSv5b";
    protected HttpClient Client { get; } 
    protected GameSyncAppFactory Factory { get; }

    public TestsWithLoggedUser(GameSyncAppFactory factory)
    {
        Client = factory.CreateClient();
        Factory = factory;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
    public async Task InitializeAsync()
    {
        await Factory.CreateConfirmedUser(Mail, Mail, Password);

        var (response, result) = await Client.POSTAsync<SignInEndpoint, SignInRequest, SuccessfulSignInResponse>(new SignInRequest { Email = Mail, Password = Password});
        Client.SetBearerToken(result!.Token);
    }
}
