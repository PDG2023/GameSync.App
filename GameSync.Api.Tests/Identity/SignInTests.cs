using Bogus.DataSets;
using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests.Identity;

[Collection("FullApp")]
public class SignInTests
{
    private readonly GameSyncAppFactory _factory;
    private readonly ITestOutputHelper output;
    private readonly HttpClient _client;

    private JsonSerializerOptions _failedSignInOpt;
    

    public SignInTests(GameSyncAppFactory integrationTestFactory, ITestOutputHelper output)
    {
        _factory = integrationTestFactory;
        this.output = output;
        _client = _factory.CreateClient();
        _failedSignInOpt = new JsonSerializerOptions
        {
            IncludeFields = true // used for sign in and ProblemDetails 
        };
    }

    [Theory]
    [InlineData("non-existing-mail@gmail.com", "")]
    public async Task LogIn_with_false_credentials_is_not_successful(string mail, string password)
    {
        // arrange
        var request = new SignInRequest { Email = mail, Password = password };

        // act
        var response = await _client.PostAsJsonAsync("api/users/sign-in", request);

        // assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errors = await response.Content.ReadFromJsonAsync<JsonObject>(_failedSignInOpt);
        Assert.NotNull(errors?["errors"]);
        var error = Assert.Single(errors!["errors"]!.AsArray());
        Assert.Equal("NotFound", error!["code"]!.ToString());
    }

    [Fact]
    public async Task LogIn_with_correct_credentials_and_unconfirmed_user_is_not_successful()
    {
        // arrange
        const string pwd = "%C$iS2z*gUZR3Hud7"; 
        var mail = new Internet().Email();
        var user = new User
        {
            Email = mail,
            UserName = mail
        };

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.Resolve<UserManager<User>>();
        await userManager.CreateAsync(user, pwd);

        var req = new SignInRequest { Email = user.Email, Password = pwd };

        // act
        var response  = await _client.PostAsJsonAsync("api/users/sign-in", req);

        // assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var errors = await response.Content.ReadFromJsonAsync<JsonObject>(_failedSignInOpt);
        Assert.NotNull(errors?["errors"]);
        var error = Assert.Single(errors["errors"]!.AsArray());
        Assert.Equal("ConfirmationNeeded", error!["code"]!.ToString());
    }

    [Fact]
    public async Task LogIn_with_correct_credentials_and_confirmed_user_is_successful_and_produce_correct_jwt()
    {
        // arrange : create an account and validate it directly
        const string password = "$UX#%A!qaphEL2";
        var mail = new Internet().Email();
        var user = new User
        {
            Email = mail,
            UserName = mail
        };

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.Resolve<UserManager<User>>();
        await userManager.CreateAsync(user, password);

        var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.ConfirmEmailAsync(user, confirmationToken);
        var configuration = _factory.Services.GetRequiredService<IConfiguration>();

        var signInRequest = new SignInRequest
        {
            Email = user.Email,
            Password = "$UX#%A!qaphEL2"
        };

        // act
        var (response, result) = await _client.POSTAsync<SignInEndpoint, SignInRequest, SuccessfulSignInResponse>(signInRequest);

        // assert
        try
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        catch
        {
            output.WriteLine(await response.Content.ReadAsStringAsync());
            throw;
        }

        Assert.NotNull(result);
        Assert.Equal(result.Email, user.Email);

        // In some cases, asp.net co+e add cookies in the header. We assert it isn't the case
        Assert.False(response.Headers.TryGetValues("Cookie", out _));

        var token = result.Token;
        _client.SetToken(JwtBearerDefaults.AuthenticationScheme, token);

        // Try accessing an authorized endpoint
        var (meResponse, meResult) = await _client.GETAsync<MeEndpoint, MeResult>();

        meResponse.EnsureSuccessStatusCode();

        // clean
        await userManager.DeleteAsync(user);
    }

}
