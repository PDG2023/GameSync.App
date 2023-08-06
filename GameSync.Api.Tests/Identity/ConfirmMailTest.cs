using Bogus.DataSets;
using FastEndpoints;
using GameSync.Api.Endpoints.Users;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameSync.Api.Tests.Identity;


[Collection("FullApp")]
public class ConfirmMailTest 
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;

    public ConfirmMailTest(GameSyncAppFactory appFactory)
    {
        _factory = appFactory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Confirm_account_with_invalid_token_produces_error()
    {
        // arrange
        const string fakeToken = "hello-world";
        var request = new ConfirmRequest { Email = "starfoullah waters", ConfirmationToken = fakeToken };

        // act
        var (response, result) = await _client.GETAsync<ConfirmEndpoint, ConfirmRequest, EmptyResponse>(request);
        
        // assert

            
     }

    [Fact]
    public async Task Confirm_existing_account_with_valid_token_enables_signin()
    {
        // arrange 
        const string pwd = "k4S4kjrC3Uv$PUoZ!";
        var mail = new Internet().Email();
       

        string token;

        using (var scope = _factory.Services.CreateScope())
        {
            var user = new User
            {
                Email = mail,
                UserName = mail
            };
            var userManager = scope.Resolve<UserManager<User>>();
            await userManager.CreateAsync(user, pwd);
            token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        }


        var request = new ConfirmRequest { ConfirmationToken = token, Email = mail };

        // act
        var (response, result) = await _client.GETAsync<ConfirmEndpoint, ConfirmRequest, EmptyResponse>(request); // confirm the mail

        // asert
        response.EnsureSuccessStatusCode();
        
        using (var scope = _factory.Services.CreateScope())
        {
            var manager = scope.Resolve<UserManager<User>>();
            var newlyAddedUser = await manager.FindByEmailAsync(mail);
            Assert.NotNull(newlyAddedUser);
            Assert.True(await manager.IsEmailConfirmedAsync(newlyAddedUser));
        }
    }
}
