using GameSync.Business.BoardGameGeek.Model;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;

namespace GameSync.Api.Tests;


[Collection("FullApp")]
public class GameSearchTests
{
    private readonly GameSyncAppFactory _factory;
    private readonly ITestOutputHelper _output;

    public GameSearchTests(GameSyncAppFactory integrationTestFactory, ITestOutputHelper output)
    {
        _factory = integrationTestFactory;
        _output = output;
    }


    [Fact]
    public async Task VimtoCluedoShouldReturnSingleResult()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/games/search?query=Vimto Cluedo&pageSize=10&page=0");
        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
        var searchResult = await response.Content.ReadFromJsonAsync<PaginatedResult<BoardGameSearchResult>>();

        Assert.Null(searchResult.PreviousPage);
        Assert.Null(searchResult.NextPage);

        var actual = Assert.Single(searchResult!.Items);
        var expected = new BoardGameSearchResult
        {
            Id = 72917,
            Name = "Vimto Cluedo",
            YearPublished = 2008,
            IsExpansion = false,
            ImageUrl = "https://cf.geekdo-images.com/5QN9cOgpqbt0YrTPgU90eA__original/img/dtSq8YNpdduYrkEp4vzeqJGKzjU=/0x0/filters:format(jpeg)/pic760405.jpg",
            ThumbnailUrl = "https://cf.geekdo-images.com/5QN9cOgpqbt0YrTPgU90eA__thumb/img/h4eG5WjhcTG2gF47Ckq_U6GkTU0=/fit-in/200x150/filters:strip_icc()/pic760405.jpg"
        };
        Assert.Equivalent(expected, actual);
      
    }
}