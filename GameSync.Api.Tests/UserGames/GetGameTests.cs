using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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
        public async Task User_without_games_should_return_an_empty_array()
        {
            var (response, result) = await Client.GETAsync<GetAllGames.Endpoint, IEnumerable<Game>>();

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
        public async Task Getting_detail_of_game_should_work()
        {
            // arrange
            var games = await Task.WhenAll(
                Factory.CreateTestGame(UserId),
                Factory.CreateTestGame(UserId)

            );
            var req = new RequestToIdentifiableObject { Id = games[0].Id };

            // act
            var (response, result) = await Client.GETAsync<GetGame.Endpoint, RequestToIdentifiableObject, Game>(req);

            // assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(result);
            Assert.Equivalent(games[0], result);
        }

        [Fact]
        public async Task Getting_games_of_user_with_two_games_should_return_them_all()
        {
            // arrange : add said games
            var expectedFirstGame = await Factory.CreateTestGame(UserId);
            var expectedSecondGame = await Factory.CreateTestGame(UserId);

            // act 
           var (response, result) = await Client.GETAsync<GetAllGames.Endpoint, IEnumerable<Game>>();

            // assert
            Assert.NotNull(result);
            response.EnsureSuccessStatusCode();
            Assert.Collection(result,
                first => AssertEquivalence(expectedFirstGame, first),
                second => AssertEquivalence(expectedSecondGame, second));
        }


        private void AssertEquivalence(Game expectedGame, Game result)
        {
            var expected = new
            {
                expectedGame.Name,
                expectedGame.MinPlayer,
                expectedGame.Description,
                expectedGame.MaxPlayer,
                expectedGame.DurationMinute
            };

            Assert.Equivalent(expected, result);
        }

    }

}


