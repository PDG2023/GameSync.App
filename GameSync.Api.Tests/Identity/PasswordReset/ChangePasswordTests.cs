using Bogus.DataSets;
using FakeItEasy;
using FastEndpoints;
using GameSync.Api.Endpoints.Users.PasswordReset;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.Identity.PasswordReset;

[Collection("FullApp")]
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
            PasswordRepetition = password,
            Token = token
        };
        // act
        var (response, result) = await _client.POSTAsync<ChangePassword.Endpoint, ChangePassword.Request, NotFound>(request);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Changing_password_of_existing_user_with_invalid_token_produces_bad_request()
    {
        // arrange 
        var mail = new Internet().Email();
        const string password = "sQ$94ju%HGxS@YhueL8cy!W";
        await _factory.CreateConfirmedUser(mail, mail, "sQ$94ju%HGxS@YhueL8cy!W");

        var request = new ChangePassword.Request
        {
            Email = mail,
            Password = password,
            PasswordRepetition = password,
            Token = "..."
        };

        // act
        var (response, result) = await _client.POSTAsync<ChangePassword.Endpoint, ChangePassword.Request, BadRequestWhateverError>(request);

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Changing_password_of_existing_user_with_valid_token_produces_OK_and_changes_it()
    {
        // arrange
        var mail = new Internet().Email();
        const string password = "!xXC%5m%eFeE!E^u&!LDTV5";
        await _factory.CreateConfirmedUser(mail, mail, password);

        string token;

        using (var scope = _factory.Services.CreateScope())
        {
            var manager = scope.Resolve<UserManager<User>>();
            var tempUser = await manager.FindByEmailAsync(mail);
            token = await manager.GeneratePasswordResetTokenAsync(tempUser!);

        }

        var request = new ChangePassword.Request
        {
            Email = mail,
            Password = password,
            PasswordRepetition = password,
            Token = token
        };

        // act
        var (response, result) = await _client.POSTAsync<ChangePassword.Endpoint, ChangePassword.Request, Ok>(request);

        // assert
        response.EnsureSuccessStatusCode();

        // try to login with the new credentials

        using var deletionScope = _factory.Services.CreateScope();
        var signInManager = deletionScope.Resolve<SignInManager<User>>();
        var user = await signInManager.UserManager.FindByEmailAsync(mail);
        var signInResult = await signInManager.CheckPasswordSignInAsync(user!, password, false);
        Assert.True(signInResult.Succeeded);

    }

}
