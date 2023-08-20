using FastEndpoints;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Org.BouncyCastle.Crypto;
using System.Net;
using Xunit;

namespace GameSync.Api.Tests.UserGames;

[Collection("FullApp")]
public class AddGameFromBggTests : TestsWithLoggedUser
{
    public AddGameFromBggTests(GameSyncAppFactory factory) : base(factory)
    {
    }


    [Fact]
    public async Task Adding_non_existing_game_from_bgg_produces_not_found()
    {

        // arrange
        const int nonExistingId = 848965651;
        var addExistingGameRequest = new AddGameFromBggRequest
        {
            IDs = new[] { nonExistingId }
        };

        // act
        var (response, result) = await Client.POSTAsync<AddGameFromBggEndpoint, Endpoints.Users.Me.Games.AddGameFromBggRequest, NotFound<IEnumerable<int>>>(addExistingGameRequest);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal($"[{nonExistingId}]", await response.Content.ReadAsStringAsync());

    }

    [Fact]
    public async Task Adding_an_existing_game_should_return_it_and_persist_it()
    {

        // arrange
        const int firstGameId = 321016;
        const int secondGameId = 34734;
        var addExistingGameRequest = new AddGameFromBggRequest
        {
            IDs = new[] { firstGameId, secondGameId }
        };

        // act
        var (response, result) = await Client.POSTAsync<AddGameFromBggEndpoint, AddGameFromBggRequest, Ok>(addExistingGameRequest);
        var (getResponse, listOfGames) = await Client.GETAsync<GetGamesEndpoint, IEnumerable<Game>>();

        // assert
        response.EnsureSuccessStatusCode();
        getResponse.EnsureSuccessStatusCode();
        Assert.NotNull(listOfGames);
        Assert.Collection(listOfGames,
            game =>
            {
                var expected = new Game
                {
                    Id = firstGameId,
                    Name = "The Thing: Norwegian Outpost",
                    MaxPlayer = 8,
                    MinPlayer = 4,
                    Description = "This expansion for The Thing - The Boardgame allows you to experience the events that happened at Thule Station, the Norwegian station where it all began! As for the 1982 version, the expansion lets you relive the tensest moments from the story, transporting you into the cinematic film!&amp;#10;&amp;#10;The core element of the game is based on the emulation properties of the Thing, which will hide its identity under a blanket of fake humanity, but with substantial differences in the way they interact with the game and with your feelings.&amp;#10;&amp;#10;Here are the key points of the expansion:&amp;#10;&amp;#10;     A new board, new characters, and new materials.&amp;#10;     A new end game condition represented by the escape of the dog that will arrive at Outpost 31. IF this happens, you can play your next game of the 1982 version with a pack leader dog (a dog that behaves differently throughout the game).&amp;#10;     A new element in the game: teeth! The Thule Station crew discovers that the Alien cannot assimilate and duplicate metal parts, so dental fillings become a way to find out if a person is definitely human, but not if they are alien. The test with the wire and the flamethrower disappears and flashlights are added with which it will be possible to carry out a test and look at the teeth of the other characters, checking for the presence or lack of fillings.&amp;#10;     A new location is available for the Thing: The UFO! It represents the last hope of escape for the Thing, as in the movie. The final confrontation with any remaining humans will take place here. Of course, this is only an option, and just like the dog escape mentioned above, it doesn't necessarily happen! Everything will depend on your behavior, as you can win or lose in many ways!&amp;#10;&amp;#10;&amp;#10;In the 2011 film, as soon as they found out what's going on, people's behavior becomes more selfish, trying by all means to save their own lives. We therefore decided to try to include the possibility of a solitary escape, an element that will bring the level of distrust to the maximum! This also brings new mechanics, such as the ability to voluntarily sabotage vehicles even if you are human.&amp;#10; The expansion will contain: a new board, new characters, new cards and new tokens.&amp;#10;&amp;#10;&amp;mdash;description from the publisher&amp;#10;&amp;#10;",
                    MinAge = 14,
                    DurationMinute = 90,
                    UserId = UserId
                };

                Assert.Equivalent(expected, game);
            },

            game =>
            {
                var expected = new Game
                {
                    Id = secondGameId,
                    Name = "Grabsch",
                    MaxPlayer = 4,
                    MinPlayer = 2,
                    Description = "Content:&amp;#10;48 picture cards&amp;#10;&amp;#10;As time runs all players try to fit cards in their hands at the same time. It is important to catch the cards as fast as lightning. The winner is the player with the most caught cards.&amp;#10;&amp;#10;",
                    MinAge = 6,
                    DurationMinute = 5,
                    UserId = UserId
                };

                Assert.Equivalent(expected, game);
            });
        

    }
}
