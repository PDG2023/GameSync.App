using Bogus.DataSets;
using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
            UserName = _signInRequest.Email
        };
    }

    [Fact]
    public async Task LogIn_with_correct_credentials_is_successful_and_produce_correct_jwt()
    {
        // arrange : create an account and validate it directly
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.Resolve<UserManager<User>>();
        await userManager.CreateAsync(_user, _password);
        var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(_user);
        await userManager.ConfirmEmailAsync(_user, confirmationToken);
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();
        
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
        Assert.Equal(result.Email, _user.Email);

        // In some cases, asp.net co+e add cookies in the header. We assert it isn't the case
        Assert.False(response.Headers.TryGetValues("Cookie", out _));

        var token = result.Token;
        _client.SetToken(JwtBearerDefaults.AuthenticationScheme, token);

        // Try accessing an authorized endpoint
        var (meResponse, meResult) = await _client.GETAsync<MeEndpoint, MeResult>();

        meResponse.EnsureSuccessStatusCode();

        // clean
        await userManager.DeleteAsync(_user);
    }



}
