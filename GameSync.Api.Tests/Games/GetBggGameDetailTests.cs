using FastEndpoints;
using GameSync.Api.CommonRequests;
using GameSync.Api.CommonResponses;
using GameSync.Api.Endpoints.Games;
using GameSync.Api.Endpoints.Users.Me.Games.FromBgg;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace GameSync.Api.Tests.Games;


[Collection("FullApp")]
public class GetBggGameDetailTests : TestsWithLoggedUser
{

    public GetBggGameDetailTests(GameSyncAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Retrieving_details_of_non_existing_game_produces_not_found()
    {
        var detailGameRequest = new RequestToIdentifiableObject { Id = 5644415 };

        var (response, _) = await Client.GETAsync<GetBggGameDetail.Endpoint, RequestToIdentifiableObject, NotFound>(detailGameRequest);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Retrieving_details_of_existing_game_in_collection_should_return_it_with_flag_enabled()
    {
        // arrange
        var detailGameRequest = new RequestToIdentifiableObject { Id = 4848 };
        var (addGameResponse, _) = await Client.POSTAsync<AddBggGame.Endpoint, RequestToIdentifiableObject, Ok>(detailGameRequest);
        addGameResponse.EnsureSuccessStatusCode();

        // act
        var (response, result) = await Client.GETAsync<GetBggGameDetail.Endpoint, RequestToIdentifiableObject, GetBggGameDetail.Response>(detailGameRequest);

        // assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(result);
        Assert.True(result.InCollection);

    }

    [Fact]
    public async Task Retrieving_details_of_existing_game_not_in_collection_should_return_it()
    {
        var detailGameRequest = new RequestToIdentifiableObject { Id = 1087 };

        var (response, result) = await Client.GETAsync<GetBggGameDetail.Endpoint, RequestToIdentifiableObject, GetBggGameDetail.Response>(detailGameRequest);

        response.EnsureSuccessStatusCode();

        var expectedResponse = new GetBggGameDetail.Response
        {
            Game = new()
            {
                Description = "In Matschig, players sling mud at one another by throwing water and sand cards. Well, okay, not literally throwing the cards, but you get the idea.&#10;&#10;To start the game, each player receives a hand of seven cards from the 110-card deck; card types are water, sand, umbrella and special. On a turn, the active player takes one sand and one water card from his hand to create mud, then hurls it at another player. (Again, not literally...) That player has a chance to defend herself by playing umbrella cards to block either the sand or water or both, or by playing special cards that, for example, redirect the mud back to the thrower or spread it out on all other players. Other players can add to someone's attack in order to make the perfect mix of mud &ndash; e.g., adding a 1-value water to a muddy mix of 5-value sand and 4-value water &ndash; and by making the perfect mix, they get to redirect the attack at any player.&#10;&#10;After the mudball hits its target or splats on someone's shield, each player refills her hand to seven cards, then the player who was hit in the previous round starts the new round by choosing a target and throwing. The game continues until the deck runs out of cards, in which case players shuffle discarded cards in order to finish the final round. Players then tally the sand and water points in front of them, and the player with the lowest score wins.&#10;&#10;",
                DurationMinute = 30,
                Id = 1087,
                ImageUrl = "https://cf.geekdo-images.com/KQTDwFXMTsDRCRIvHybXaQ__original/img/XEhrW3JkfuR1a8VlgGH9c3n8z-U=/0x0/filters:format(jpeg)/pic1197487.jpg",
                MaxPlayer = 6,
                MinAge = 8,
                MinPlayer = 3,
                Name = "Matschig",
                IsExpansion = false,
                ThumbnailUrl = "https://cf.geekdo-images.com/KQTDwFXMTsDRCRIvHybXaQ__thumb/img/-tVDyCw7Gl3CUOyN99bJ8L6t2eg=/fit-in/200x150/filters:strip_icc()/pic1197487.jpg",
                YearPublished = 1998
            },
            InCollection = false
        };

        Assert.Equivalent(expectedResponse, result);
    }

}
