using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace GameSync.Api.Tests.UserGames;



public class GetGameTests
{

    [Collection("FullApp")]
    public class UserWithoutGames : TestsWithLoggedUser
    {
        public UserWithoutGames(GameSyncAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async void User_without_games_should_return_an_empty_array()
        {
            var (response, result) = await Client.GETAsync<GetGamesEndpoint, IEnumerable<Game>>();

            Assert.NotNull(result);
            response.EnsureSuccessStatusCode();
            Assert.Empty(result);
        }
    }



    [Collection("FullApp")]
    public class UserWithGames : TestsWithLoggedUser
    {

        public UserWithGames(GameSyncAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async void Getting_games_of_user_with_two_games_should_return_them_all()
        {
            // arrange : add said games
            var games = await Task.WhenAll(
                Factory.CreateTestGame(UserId, 50),
                Factory.CreateTestGame(UserId, 51)
            );

            // act 
           var (response, result) = await Client.GETAsync<GetGamesEndpoint, IEnumerable<Game>>();

            // assert
            Assert.NotNull(result);
            response.EnsureSuccessStatusCode();
            Assert.Collection(result,
                first => Assert.Equal(50, first.Id),
                second => Assert.Equal(51, second.Id));
        }
    }

}


