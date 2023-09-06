using GameSync.Api.BoardGameGeek;
using GameSync.Api.CommonResponses;
using Xunit;

namespace Tests.BoardGameGeek;

public class BoardGameGeekClientTests
{
    private readonly BoardGameGeekClient _client = new();

    [Fact]
    public async Task Retrieving_vimeo_cluedo_should_work()
    {

        var result = await _client.SearchBoardGamesAsync("Vimto Cluedo");

        var expected = new GamePreview
        {
            Id = 72917,
            Name = "Vimto Cluedo",
            YearPublished = 2008,
            IsExpansion = false,
            ImageUrl = "https://cf.geekdo-images.com/5QN9cOgpqbt0YrTPgU90eA__original/img/dtSq8YNpdduYrkEp4vzeqJGKzjU=/0x0/filters:format(jpeg)/pic760405.jpg",
            ThumbnailUrl = "https://cf.geekdo-images.com/5QN9cOgpqbt0YrTPgU90eA__thumb/img/h4eG5WjhcTG2gF47Ckq_U6GkTU0=/fit-in/200x150/filters:strip_icc()/pic760405.jpg"
        };
        Assert.Equivalent(expected, Assert.Single(result));
    }

    [Fact]
    public async Task Retrieving_detail_of_vimeo_cluedo_should_work()
    {
        var result = await _client.GetBoardGamesDetailAsync(new[] { 72917 });

        var game = Assert.Single(result);
        Assert.Equal("Vimto Cluedo", game.Name);
        Assert.Equal(3, game.MinPlayer);
        Assert.Equal(6, game.MaxPlayer);
        Assert.Equal(8, game.MinAge);
        Assert.Equal(0, game.DurationMinute);

    }

    [Fact]
    public async Task Retrieving_non_existing_game_should_return_empty_collection()
    {
        var result = await _client.GetBoardGamesDetailAsync(new[] { 498445616 });
        Assert.Empty(result);

    }

}
