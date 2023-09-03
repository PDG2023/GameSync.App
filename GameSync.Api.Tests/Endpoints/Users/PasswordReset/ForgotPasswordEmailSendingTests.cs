using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.PasswordReset;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Tests.Mocks;
using Xunit;

namespace Tests.Endpoints.Users.PasswordReset;

[Collection("FullApp")]
public class ForgotPasswordEmailSendingTests
{
    private readonly GameSyncAppFactory _factory;
    public ForgotPasswordEmailSendingTests(GameSyncAppFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Service_unavailable_is_sent_when_there_is_an_error()
    {
        // arrange
        var mail = new Bogus.DataSets.Internet().Email();
        await _factory.CreateConfirmedUserAsync(mail, mail, "Q9d&h@T6jtQBWwaivWq4@JM");

        var request = new RequestToUser { Email = mail };

        var mockMailService = new MockMailService(true);

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.Resolve<UserManager<User>>();

        // act
        var result = await new ForgotPassword.Endpoint(userManager, mockMailService).ExecuteAsync(request, CancellationToken.None);
        var serviceUnavailableResult = result.Result as StatusCodeHttpResult;

        // assert
        Assert.NotNull(serviceUnavailableResult);
        Assert.Equal((int)HttpStatusCode.ServiceUnavailable, serviceUnavailableResult.StatusCode);
    }

    [Fact]
    public async Task Mail_is_sent_when_user_exists()
    {
        // arrange
        var mail = new Bogus.DataSets.Internet().Email();
        await _factory.CreateConfirmedUserAsync(mail, mail, "Q9d&h@T6jtQBWwaivWq4@JM");

        var request = new RequestToUser { Email = mail };

        var mockMailService = new MockMailService(false);

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.Resolve<UserManager<User>>();

        // act
        var result = await new ForgotPassword.Endpoint(userManager, mockMailService).ExecuteAsync(request, CancellationToken.None);
        var okResult = result.Result as Ok;

        // assert
        Assert.NotNull(okResult);

        // checks that the mail has been correctly sent
        Assert.True(mockMailService.Mails.ContainsKey(mail));

    }

    [Fact]
    public async Task Mail_is_not_sent_when_user_does_not_exist()
    {
        // arrange 
        var mail = new Bogus.DataSets.Internet().Email();
        var request = new RequestToUser { Email = mail };
        var mockMailService = new MockMailService(false);
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.Resolve<UserManager<User>>();

        // act
        var result = await new ForgotPassword.Endpoint(userManager, mockMailService).ExecuteAsync(request, CancellationToken.None);
        var okResult = result.Result as Ok;

        // assert
        Assert.NotNull(okResult);
        Assert.False(mockMailService.Mails.ContainsKey(mail));

    }

}
