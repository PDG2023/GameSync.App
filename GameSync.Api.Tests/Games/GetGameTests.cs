using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Games;
using GameSync.Business.BoardGameGeek.Model;
using GameSync.Business.BoardGamesGeek;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace GameSync.Api.Tests.Games;

[Collection("FullApp")]
public class GetGameTests
{
    private readonly GameSyncAppFactory _factory;
    private readonly HttpClient _client;
    public GetGameTests(GameSyncAppFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Retrieving_details_of_non_existing_game_produces_not_found()
    {
        var detailGameRequest = new RequestToIdentifiableObject { Id = 5644415 };

        var (response, result) = await _client.GETAsync<GetGameEndpoint, RequestToIdentifiableObject, NotFound>(detailGameRequest);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Retrieving_details_of_existing_game_should_return_it()
    {
        var detailGameRequest = new RequestToIdentifiableObject { Id = 1087 };

        var (response, result) = await _client.GETAsync<GetGameEndpoint, RequestToIdentifiableObject, BoardGameGeekGame>(detailGameRequest);

        response.EnsureSuccessStatusCode();

        var expected = new BoardGameGeekGame
        {
            Description = "In Matschig, players sling mud at one another by throwing water and sand cards. Well, okay, not literally throwing the cards, but you get the idea.&#10;&#10;To start the game, each player receives a hand of seven cards from the 110-card deck; card types are water, sand, umbrella and special. On a turn, the active player takes one sand and one water card from his hand to create mud, then hurls it at another player. (Again, not literally...) That player has a chance to defend herself by playing umbrella cards to block either the sand or water or both, or by playing special cards that, for example, redirect the mud back to the thrower or spread it out on all other players. Other players can add to someone's attack in order to make the perfect mix of mud &ndash; e.g., adding a 1-value water to a muddy mix of 5-value sand and 4-value water &ndash; and by making the perfect mix, they get to redirect the attack at any player.&#10;&#10;After the mudball hits its target or splats on someone's shield, each player refills her hand to seven cards, then the player who was hit in the previous round starts the new round by choosing a target and throwing. The game continues until the deck runs out of cards, in which case players shuffle discarded cards in order to finish the final round. Players then tally the sand and water points in front of them, and the player with the lowest score wins.&#10;&#10;",
            DurationMinute = 30,
            Id = 1087,
            ImageUrl = "https://cf.geekdo-images.com/KQTDwFXMTsDRCRIvHybXaQ__original/img/XEhrW3JkfuR1a8VlgGH9c3n8z-U=/0x0/filters:format(jpeg)/pic1197487.jpg",
            MaxPlayer = 6,
            MinAge = 8,
            MinPlayer = 3,
            Name = "Matschig"
        };
        Assert.Equivalent(expected, result);
    }

}
