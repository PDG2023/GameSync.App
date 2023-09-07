using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users;
using IdentityModel.Client;
using Xunit;

namespace Tests;

public class TestsWithLoggedUser : IAsyncLifetime
{

    protected string Mail { get; } = new Bogus.DataSets.Internet().Email();
    protected string Password { get; } = "uPY994@euuK9&TPny#wSv5b";
    public string UserId { get; private set; }
    public HttpClient Client { get; }
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
    public virtual async Task InitializeAsync()
    {
        UserId = await Factory.CreateConfirmedUserAsync(Mail, Mail, Password);
        var (_, result) = await Client.POSTAsync<SignIn.Endpoint, RequestWithCredentials, SignIn.Response>(new RequestWithCredentials { Email = Mail, Password = Password });
        Client.SetBearerToken(result!.Token);
    }
}
