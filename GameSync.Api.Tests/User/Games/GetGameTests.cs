using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GameSync.Api.Tests.EntityUser.Games;



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
            using var scope = Factory.Services.CreateScope();
            var user = await scope.Resolve<UserManager<User>>().FindByEmailAsync(Mail);
            var games = new List<Game>
            {
                GetTestGame(user.Id, "first"),
                GetTestGame(user.Id, "second"),
            };
            var ctx = scope.Resolve<GameSyncContext>();
            await ctx.Games.AddRangeAsync(games);
            await ctx.SaveChangesAsync();

            // act 
            var (response, result) = await Client.GETAsync<GetGamesEndpoint, IEnumerable<Game>>();

            // assert
            Assert.NotNull(result);
            response.EnsureSuccessStatusCode();
            Assert.Collection(result,
                first => Assert.Equal("first", first.Name),
                second => Assert.Equal("second", second.Name));
        }


        private static Game GetTestGame(string userId, string name) => new Game
        {
            MaxPlayer = 5,
            MinAge = 5,
            MinPlayer = 5,
            Name = name,
            UserId = userId
        };
    }


}


