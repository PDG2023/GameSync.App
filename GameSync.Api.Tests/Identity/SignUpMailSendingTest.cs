using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using GameSync.Business.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var endpoint = new SignUpEndpoint(userManager, mockService);

        // act
        var response = await endpoint.ExecuteAsync(TestRequest, CancellationToken.None);

        var result = (response.Result as BadRequest<ProblemDetails>)?.Value;

        // assert
        Assert.NotNull(result);
        var errorCode = Assert.Single(result.Errors).Code;
        Assert.Equal("MailNotSend", errorCode);

    }

    [Fact]
    public async Task Creating_new_correct_account_should_send_mail()
    {
        // arrange
        var mockService = new MockMailService(false);

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var endpoint = new SignUpEndpoint(userManager, mockService);

        // act
        var response = await endpoint.ExecuteAsync(TestRequest, CancellationToken.None);
        var status = (Ok<SuccessfulSignUpResponse>)response.Result;
        var result = status.Value;

        // assert
        var addedMail = Assert.Single(mockService.Mails).Key;
        Assert.Equal(TestRequest.Email, addedMail);
    }

    private static SignUpRequest TestRequest { get; } =  new()
    {
        Email = new Bogus.DataSets.Internet().Email(),
        Password = "%7#FMe*ArfFLWb4h2"
    };

}

