using Bogus.DataSets;
using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Endpoints.Users.Me;
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
        await _factory.CreateUnconfirmedUser(mail, mail, pwd);

        var req = new SignInRequest { Email = mail, Password = pwd };

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
        await _factory.CreateConfirmedUser(mail, mail, password);
        var signInRequest = new SignInRequest
        {
            Email = mail,
            Password = password
        };

        // act
        var (response, result) = await _client.POSTAsync<SignInEndpoint, SignInRequest, SuccessfulSignInResponse>(signInRequest);

        // assert
        await response.EnsureSuccessAndDumpBodyIfNotAsync(output);

        Assert.NotNull(result);
        Assert.Equal(result.Email, mail);

        // In some cases, asp.net co+e add cookies in the header. We assert it isn't the case
        Assert.False(response.Headers.TryGetValues("Cookie", out _));

        var token = result.Token;
        _client.SetToken(JwtBearerDefaults.AuthenticationScheme, token);

        // Try accessing an authorized endpoint
        var (meResponse, meResult) = await _client.GETAsync<MeEndpoint, MeResult>();

        meResponse.EnsureSuccessStatusCode();

    }

}
