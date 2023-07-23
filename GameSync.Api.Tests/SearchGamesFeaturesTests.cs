using GameSync.Api.Persistence;
using GameSync.Business.Features.Search;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GameSync.Api.Tests
{
    public class SearchGamesFeaturesTests : IClassFixture<GameSyncAppFactory>
    {
        private readonly GameSyncAppFactory _factory;

        public SearchGamesFeaturesTests(GameSyncAppFactory integrationTestFactory)
        {
            _factory = integrationTestFactory;

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
            var response = await _factory.Client.GetAsync("/api/search?term=Loups");

            // assert
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadFromJsonAsync<IEnumerable<Game>>();
            Assert.Equal(2, res.Count());
        }
    }
}