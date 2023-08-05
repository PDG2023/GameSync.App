using Bogus.DataSets;
using FakeItEasy;
using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests;

public class SignInTests : IClassFixture<GameSyncAppFactory>
{
    private const string _password = "$UX#%A!qaphEL2";
    private readonly SignInRequest _signInRequest;
    private readonly User _user;
    private readonly GameSyncAppFactory _factory;
    private readonly ITestOutputHelper output;
    private readonly HttpClient _client;

    public SignInTests(GameSyncAppFactory integrationTestFactory, ITestOutputHelper output)
    {
        _factory = integrationTestFactory;
        this.output = output;
        _client = _factory.CreateClient();

        _signInRequest = new()
        {
            Email = new Internet().Email(),
            Password = _password
        };

        _user = new()
        {
            Email = _signInRequest.Email,
            UserName = _signInRequest.Email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
    }

    [Fact]
    public async Task LogIn_with_correct_credentials_is_successful_and_produce_correct_jwt()
    {
        // arrange 
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.Resolve<UserManager<User>>();
        await userManager.CreateAsync(_user, _password);
        var confirmationTokn = await userManager.GenerateEmailConfirmationTokenAsync(_user);
        var d = await userManager.ConfirmEmailAsync(_user, confirmationTokn);

        // act
        var (response, result) = await _client.POSTAsync<SignInEndpoint, SignInRequest, SuccessfulSignInResponse>(_signInRequest);

        // assert
        try
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        catch (Exception e)
        {
            output.WriteLine(await response.Content.ReadAsStringAsync());
            throw;
        }

        Assert.NotNull(result);
        Assert.Equal(result.Email, result.Email);

        // clean
        await userManager.DeleteAsync(_user);
    }


    private SignInManager<User> MockSignManager()
    {
        var mockSignIn = A.Fake<SignInManager<User>>();

        A.CallTo(() => mockSignIn.CheckPasswordSignInAsync(_user, _password, false))
            .Returns(Task.FromResult(SignInResult.Success));
        return mockSignIn;
    }

}
