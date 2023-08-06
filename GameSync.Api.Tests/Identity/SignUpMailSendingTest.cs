using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.Identity;



[Collection("FullApp")]
public class SignUpMailSendingTest
{

    private readonly GameSyncAppFactory _factory;

    public SignUpMailSendingTest(GameSyncAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Disabled_mail_service_should_produce_errors()
    {
        // arrange
        var mockService = new MockMailService(true);
        var req = CreateTestUser();

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var endpoint = new SignUpEndpoint(userManager, mockService);

        // act
        var response = await endpoint.ExecuteAsync(req, CancellationToken.None);

        var result = response.Result as StatusCodeHttpResult;

        // assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.ServiceUnavailable, result.StatusCode);

        // check that the user has been correctly deleted
        Assert.Null(await userManager.FindByEmailAsync(req.Email));
    }

    [Fact]
    public async Task Creating_new_correct_account_should_send_mail()
    {
        // arrange
        var mockService = new MockMailService(false);
        var req = CreateTestUser();

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var endpoint = new SignUpEndpoint(userManager, mockService);

        // act
        var response = await endpoint.ExecuteAsync(req, CancellationToken.None);
        var status = (Ok<SuccessfulSignUpResponse>)response.Result;
        var result = status.Value;

        // assert
        var addedMail = Assert.Single(mockService.Mails).Key;
        Assert.Equal(req.Email, addedMail);
    }

    private static SignUpRequest CreateTestUser() => new SignUpRequest
    {
        Email = new Bogus.DataSets.Internet().Email(),
        Password = "%7#FMe*ArfFLWb4h2"
    };

}

