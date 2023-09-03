using Bogus.DataSets;
using FakeItEasy;
using FastEndpoints;
using GameSync.Api;
using GameSync.Api.Endpoints.Users.PasswordReset;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace Tests.Endpoints.Users.PasswordReset;

[Collection(GameSyncAppFactoryFixture.Name)]
public class ChangePasswordTests
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;

    public ChangePasswordTests(GameSyncAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async Task Changing_password_of_non_existing_mail_produces_not_found()
    {
        // arrange
        var mail = new Internet().Email();
        const string password = "!b&Yv3yy&ki!*nKhZF2yor8";
        const string token = "...";
        var request = new ChangePassword.Request
        {
            Email = mail,
            Password = password,
            ConfirmPassword = password,
            Token = token
        };
        // act
        var (response, _) = await _client.POSTAsync<ChangePassword.Endpoint, ChangePassword.Request, NotFound>(request);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Changing_password_of_existing_user_with_invalid_token_produces_bad_request()
    {
        // arrange 
        var mail = new Internet().Email();
        const string password = "sQ$94ju%HGxS@YhueL8cy!W";
        await _factory.CreateConfirmedUserAsync(mail, mail, "sQ$94ju%HGxS@YhueL8cy!W");

        var request = new ChangePassword.Request
        {
            Email = mail,
            Password = password,
            ConfirmPassword = password,
            Token = "..."
        };

        // act
        var (response, result) = await _client.POSTAsync<ChangePassword.Endpoint, ChangePassword.Request, BadRequestWhateverError>(request);

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


}
