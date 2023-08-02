using GameSync.Business.BoardGamesGeek;
using GameSync.Business.Features.Search;
using Xunit;

namespace GameSync.Api.Tests;

public class BoardGameGeekClientTest
{
    [Fact]
    public async Task SearchVimtoCluedoReturnsSingleGame()
    {
        var bggClient = new BoardGameGeekClient();
        var result = await bggClient.SearchBoardGamesAsync("Vimto Cluedo");
        
        var actual = Assert.Single(result);
        var expected = new BoardGameSearchResult("Vimto Cluedo", 72917, false);
        Assert.Equivalent(expected, actual);
    }
}
