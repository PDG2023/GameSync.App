using Bogus.DataSets;
using GameSync.Api.Endpoints.Users;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests;

public class SignUpTests : IClassFixture<GameSyncAppFactory>
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
        var testRequest = GetTestRequest("$UX#%A!qaphEL2");

        // act
        var response = await TryCreateAccount(testRequest);
        
        // assert
        var payload = await response.Content.ReadFromJsonAsync<SignUpValidResponse>();
        Assert.NotNull(payload);
        Assert.Equal(testRequest.Email, payload.Email);

    }


    [Theory]
    [InlineData("asfWJIj1421", "PasswordRequiresNonAlphanumeric")]
    [InlineData("$1241245ds125", "PasswordRequiresUpper")]
    [InlineData("$XaBaBweqgRw", "PasswordRequiresDigit")]
    [InlineData("$X3%A!q", "PasswordTooShort")]
    public async Task BadlyFormedPassword(string password, string expectedError)
    {
        // arrange
        var testRequest = GetTestRequest(password);

        // act
        var response = await TryCreateAccount(testRequest);

        // assert
        await AssertProduceError(expectedError, response);
    }

    [Fact]
    public async Task BadlyFormedMail()
    {
        var badlyFormedMailSignInRequest = new SignUpRequest { Email = "ab", Password = "$UX#%A!qaphEL2a23" };

        var response = await TryCreateAccount(badlyFormedMailSignInRequest);

        await AssertProduceError("InvalidEmail", response);
    }


    private static SignUpRequest GetTestRequest(string password) => new SignUpRequest
    {
        Email = new Internet().Email(),
        Password = password
    };

    private async Task<HttpResponseMessage> TryCreateAccount(SignUpRequest req)
    {
        var client = _factory.CreateClient();
        return await client.PostAsJsonAsync("/api/users/sign-in", req);
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
