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

    private class MockMailService : IAuthMailService
    {
        public readonly Dictionary<string, string> _mailStore = new();

        public Task<bool> SendEmailConfirmation(string toEmail, string mailConfirmationToken)
        {
            _mailStore[toEmail] = mailConfirmationToken;
            return Task.FromResult(true);
        }
    }

    private readonly GameSyncAppFactory _factory;

    public SignUpMailSendingTest(GameSyncAppFactory factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task Creating_new_correct_account_should_send_mail()
    {
        // arrange
        var mockService = new MockMailService();
        var request = new SignUpRequest
        {
            Email = new Bogus.DataSets.Internet().Email(),
            Password = "%7#FMe*ArfFLWb4h2"
        };

        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var endpoint = new SignUpEndpoint(userManager, mockService);

        // act
        var response = await endpoint.ExecuteAsync(request, CancellationToken.None);
        var status = (Ok<SuccessfulSignUpResponse>)response.Result;
        var result = status.Value;

        // assert
        var addedMail = Assert.Single(mockService._mailStore).Value;
        Assert.Equal(request.Email, addedMail);
    }

}

