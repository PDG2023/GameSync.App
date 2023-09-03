using Bogus.DataSets;
using FastEndpoints;
using GameSync.Api;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Endpoints.Users;


[Collection(GameSyncAppFactoryFixture.Name)]
public class SignUpTests
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public SignUpTests(GameSyncAppFactory integrationTestFactory, ITestOutputHelper output)
    {
        _factory = integrationTestFactory;
        _client = _factory.CreateClient();
        _output = output;
    }


    [Fact]
    public async Task Newly_created_account_is_not_confirmed()
    {
        // arrange
        var newAccountRequest = GetNewAccountRequest("Ws%uf^n7iB9nK#e&b");

        // act
        var (response, _) = await _client.POSTAsync<SignUp.Endpoint, SignUp.Request, BadRequestWhateverError>(newAccountRequest);

        // assert
        response.EnsureSuccessStatusCode();
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(newAccountRequest.Email);
        Assert.False(await userManager.IsEmailConfirmedAsync(user!));

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
        var testResult = await _client.POSTAsync<SignUp.Endpoint, SignUp.Request, BadRequestWhateverError>(testRequest);

        // assert
        AssertProduceError(expectedError, testResult);
    }


    [Fact]
    public async Task Badly_formed_mail_produces_an_error()
    {
        var badlyFormMail = new SignUp.Request { Email = "ab", Password = "$UX#%A!qaphEL2a23", UserName = "cd" };

        var testResult = await _client.POSTAsync<SignUp.Endpoint, SignUp.Request, BadRequestWhateverError>(badlyFormMail);

        AssertProduceError("InvalidEmail", testResult);
    }


    [Fact]
    public async Task Multiple_user_with_same_username_can_exist()
    {
        var firstUser = GetNewAccountRequest("wE%LASrT4Nx25FeY^z#b^*@");
        var secondUser = new SignUp.Request
        {
            Password = firstUser.Password,
            UserName = firstUser.UserName,
            Email = new Internet().Email()
        };


        var (firstResponse, firstResult) = await _client.POSTAsync<SignUp.Endpoint, SignUp.Request, SignUp.Response>(firstUser);
        var (secondResponse, secondResult) = await _client.POSTAsync<SignUp.Endpoint, SignUp.Request, SignUp.Response>(secondUser);

        firstResponse.EnsureSuccessStatusCode();
        await secondResponse.EnsureSuccessAndDumpBodyIfNotAsync(_output);


        Assert.NotNull(firstResult);
        Assert.NotNull(secondResult);

        Assert.Equal(firstUser.UserName, firstResult.UserName);
        Assert.Equal(firstUser.UserName, secondResult.UserName);
    }

    private static SignUp.Request GetNewAccountRequest(string password) => new()
    {
        Email = new Internet().Email(),
        Password = password,
        UserName = new Name().FirstName(),
    };

    private static void AssertProduceError(string errorCode, TestResult<BadRequestWhateverError>? testResult)
    {
        Assert.NotNull(testResult);

        var (response, result) = testResult;

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);

        var error = Assert.Single(result.Errors);
        Assert.Equal(errorCode, error.Code);
    }
}
