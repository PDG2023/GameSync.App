using GameSync.Api.Persistence;
using GameSync.Business.BoardGamesGeek;
using GameSync.Business.Features.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GameSync.Api.Tests
{
    public class GameSearchTests : IClassFixture<GameSyncAppFactory>
    {
        private readonly GameSyncAppFactory _factory;

        public GameSearchTests(GameSyncAppFactory integrationTestFactory)
        {
            _factory = integrationTestFactory;

        }


        [Fact]
        public async Task SingleResult()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/games/search?query=Vimto Cluedo");
            response.EnsureSuccessStatusCode();
            var searchResult = await response.Content.ReadFromJsonAsync<IEnumerable<BoardGameSearchResult>>();

            var actual = Assert.Single(searchResult);
            var expected = new BoardGameSearchResult
            {
                Id = 72917,
                Name = "Vimto Cluedo",
                YearPublished = 2008,
                IsExpansion = false
            };
            Assert.Equivalent(expected, actual);
          
        }
    }
}