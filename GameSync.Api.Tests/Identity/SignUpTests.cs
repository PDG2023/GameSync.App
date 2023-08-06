using Bogus.DataSets;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests.Identity;


[Collection("FullApp")]
public class SignUpTests
{
    private readonly GameSyncAppFactory _factory;

    public SignUpTests(GameSyncAppFactory integrationTestFactory)
    {
        _factory = integrationTestFactory;
    }


    [Fact]
    public async Task SecurePassword_ProduceNoError()
    {
        // arrange
        var testRequest = GetNewAccountRequest("$UX#%A!qaphEL2");

        // act
        var response = await SendAccountCreationRequest(testRequest);

        // assert
        var payload = await response.Content.ReadFromJsonAsync<SucessfulSignUpResponse>();
        Assert.NotNull(payload);
        Assert.Equal(testRequest.Email, payload.Email);

    }

    [Fact]
    public async Task Newly_created_account_is_not_confirmed()
    {
        // arrange
        var newAccountRequest = GetNewAccountRequest("Ws%uf^n7iB9nK#e&b");

        // act
        var response = await SendAccountCreationRequest(newAccountRequest);

        // assert
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(newAccountRequest.Email);
        Assert.False(await userManager.IsEmailConfirmedAsync(user));

    }

    [Theory]
    [InlineData("asfWJIj1421", "PasswordRequiresNonAlphanumeric")]
    [InlineData("$1241245ds125", "PasswordRequiresUpper")]
    [InlineData("$XaBaBweqgRw", "PasswordRequiresDigit")]
    [InlineData("$X3%A!q", "PasswordTooShort")]
    public async Task Badly_formed_password_produces_an_error(string password, string expectedError)
    {
        // arrange
        var testRequest = GetNewAccountRequest(password);

        // act
        var response = await SendAccountCreationRequest(testRequest);

        // assert
        await AssertProduceError(expectedError, response);
    }



    [Fact]
    public async Task Badly_formed_mail_produces_an_error()
    {
        var badlyFormMail = new SignUpRequest { Email = "ab", Password = "$UX#%A!qaphEL2a23" };

        var response = await SendAccountCreationRequest(badlyFormMail);

        await AssertProduceError("InvalidEmail", response);
    }


    private static SignUpRequest GetNewAccountRequest(string password) => new()
    {
        Email = new Internet().Email(),
        Password = password
    };

    private async Task<HttpResponseMessage> SendAccountCreationRequest(SignUpRequest req)
    {
        var client = _factory.CreateClient();
        return await client.PostAsJsonAsync("/api/users/sign-up", req);
    }

    private async Task AssertProduceError(string errorCode, HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var typedResponse = await response.Content.ReadFromJsonAsync<IEnumerable<IdentityError>>();
        Assert.NotNull(typedResponse);
        var error = Assert.Single(typedResponse);
        Assert.Equal(errorCode, error.Code);
    }


}
