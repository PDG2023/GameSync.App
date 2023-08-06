using Bogus.DataSets;
using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameSync.Api.Tests.Identity;


public class FullScenarioTest : IClassFixture<GameSyncAppFactory>
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;

    public FullScenarioTest(GameSyncAppFactory appFactory)
    {
        _factory = appFactory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Confirm_existing_account_with_token_enables_signin()
    {
        // arrange 
        //const string pwd = "k4S4kjrC3Uv$PUoZ!";
        //var mail = new Internet().Email();
        //var user = new User
        //{
        //    Email = mail,
        //    UserName = mail
        //};

        //using var scope = _factory.Services.CreateScope();
        //var userManager = scope.Resolve<UserManager<User>>();
        //await userManager.CreateAsync(user, pwd);

        //var request = new ConfirmRequest
        //{
        //    ConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user),
        //};

        //var signRequest = new SignInRequest
        //{
        //    Email = mail,
        //    Password = pwd
        //};

        //// act
        //var token = (await _client.POSTAsync<SignInEndpoint, SignInRequest, SuccessfulSignInResponse>(signRequest)).Result.Token;
        //var (firstMeResponse, firstMeResult) = await _client.GETAsync<MeEndpoint, MeResult>(); // ensure that originally, the /me endpoint is restricted
        //var (response, result) = await _client.POSTAsync<ConfirmEndpoint, ConfirmRequest, ConfirmResult>(request); // confirm the mail
        //var (meResponse, meResult) = await _client.GETAsync<MeEndpoint, MeResult>(); // access a restricted point

        //// asert
        //Assert
        //response.EnsureSuccessStatusCode();

        //// try accessing a restricted endpoint 

        //Assert.NotNull(meResponse);
        //meResponse.EnsureSuccessStatusCode();

    }

}
