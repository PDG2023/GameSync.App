﻿using GameSync.Business.BoardGamesGeek;
using GameSync.Business.Search;
using Xunit;

namespace GameSync.Api.Tests;

public class BoardGameGeekClientTests
{
    private BoardGameGeekClient _client = new();

    [Fact]
    public async Task Retrieving_vimeo_cluedo_should_work()
    {

        var result = await _client.SearchBoardGamesAsync("Vimto Cluedo");

        var expected = new BoardGameSearchResult
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
}