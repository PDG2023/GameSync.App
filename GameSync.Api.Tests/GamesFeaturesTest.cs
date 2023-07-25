using GameSync.Api.Persistence;
using GameSync.Business.Features.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GameSync.Api.Tests
{
    public class GamesFeaturesTest : IClassFixture<GameSyncAppFactory>
    {
        private readonly GameSyncAppFactory _factory;

        public GamesFeaturesTest(GameSyncAppFactory integrationTestFactory)
        {
            _factory = integrationTestFactory;

        }

        [Fact]
        public async Task AddSingleGameTest()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<GameSyncContext>();
                await ctx.Games.ExecuteDeleteAsync();
                ctx.SaveChanges();
            }

            // act
            var response = await _factory.Client.PostAsync("/api/game/Cluedo", null);

            //assert
            response.EnsureSuccessStatusCode();
            var game = await response.Content.ReadFromJsonAsync<Game>();
            Assert.Equal("Cluedo", game.Name);
        }

        [Fact]
        public async Task SearchingNonExistingTerm()
        {

            // arrange
            using (var scope = _factory.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<GameSyncContext>();
                ctx.Games.AddRange(new[] {
                    new Persistence.Entities.Game {Name = "Loups-garous pour une nuit"},
                    new Persistence.Entities.Game {Name = "Loups-garous de thiercelieu"},
                    new Persistence.Entities.Game {Name = "Twister"},
                });
                ctx.SaveChanges();
            }

            // act
            var response = await _factory.Client.GetAsync("/api/game/search?term=Loups");

            // assert
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadFromJsonAsync<IEnumerable<Game>>();
            Assert.Equal(2, res.Count());
        }
    }
}