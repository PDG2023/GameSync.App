using Bogus.DataSets;
using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using GameSync.Business.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests.Identity;


[Collection("FullApp")]
public class ConfirmMailTest 
{
    private readonly GameSyncAppFactory _factory;
    private readonly ITestOutputHelper _output;
    private readonly HttpClient _client;

    public ConfirmMailTest(GameSyncAppFactory appFactory, ITestOutputHelper output)
    {
        _factory = appFactory;
        this._output = output;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task HttpGet_request_to_generated_link_should_confirm_a_user()
    {

        // arramge
        var mail = new Internet().Email();

        string token = await CreateUserAndGetToken(mail);
        using var scope = _factory.Services.CreateScope();
        var linkProvider = scope.ServiceProvider.GetRequiredService<ConfirmationMailLinkProvider>();

        // act
        var confirmMailLink = linkProvider.GetConfirmationMailLink(mail, token, "http", "localhost");
        var response = await _client.GetAsync(confirmMailLink);

        // assert
        try
        {
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
        catch (Exception ex)
        {
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            _output.WriteLine(token);
            throw;
        }
        await EnsureMailConfirmed(mail);

    }

    private async Task EnsureMailConfirmed(string mail)
    {
        using var scope = _factory.Services.CreateScope();
        var manager = scope.Resolve<UserManager<User>>();
        var newlyAddedUser = await manager.FindByEmailAsync(mail);
        Assert.NotNull(newlyAddedUser);
        Assert.True(await manager.IsEmailConfirmedAsync(newlyAddedUser));
    }

    private async Task<string> CreateUserAndGetToken(string mail)
    {
        string token;

        using (var scope = _factory.Services.CreateScope())
        {
            var user = new User
            {
                Email = mail,
                UserName = mail
            };
            var userManager = scope.Resolve<UserManager<User>>();
            await userManager.CreateAsync(user, "k4S4kjrC3Uv$PUoZ!");
            token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        }

        return token;
    }
}
