using GameSync.Business.Search;
using System.Net.Http.Json;
using Xunit;

namespace GameSync.Api.Tests;


[Collection("FullApp")]
public class GameSearchTests
{
    private readonly GameSyncAppFactory _factory;

    public GameSearchTests(GameSyncAppFactory integrationTestFactory)
    {
        _factory = integrationTestFactory;

    }


    [Fact]
    public async Task VimtoCluedoShouldReturnSingleResult()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/games/search?query=Vimto Cluedo&pageSize=10&page=0");
        response.EnsureSuccessStatusCode();
        var searchResult = await response.Content.ReadFromJsonAsync<PaginatedResult<BoardGameSearchResult>>();

        Assert.Null(searchResult.PreviousPage);
        Assert.Null(searchResult.NextPage);

        var actual = Assert.Single(searchResult!.Items);
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