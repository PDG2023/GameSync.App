﻿using FastEndpoints;
using GameSync.Api;
using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Endpoints.Users.Me.Games.FromBgg;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Endpoints.Users.Games;


[Collection(GameSyncAppFactoryFixture.Name)]
public class AddGameFromBggTests : TestsWithLoggedUser
{
    private readonly ITestOutputHelper _output;

    public AddGameFromBggTests(GameSyncAppFactory factory, ITestOutputHelper output) : base(factory)
    {
        _output = output;
    }


    [Fact]
    public async Task Adding_non_existing_game_from_bgg_produces_not_found()
    {

        // arrange
        const int nonExistingId = 848965651;
        var addExistingGameRequest = new RequestToIdentifiableObject
        {
            Id = nonExistingId
        };

        // act
        var (response, _) = await Client.POSTAsync<AddBggGame.Endpoint, RequestToIdentifiableObject, NotFound>(addExistingGameRequest);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Adding_twice_same_game_should_produce_bad_request()
    {
        var addExistingGameRequest = new RequestToIdentifiableObject
        {
            Id = 1421
        };


        var (response, _) = await Client.POSTAsync<AddBggGame.Endpoint, RequestToIdentifiableObject, Ok>(addExistingGameRequest);
        var (secondResponse, _) = await Client.POSTAsync<AddBggGame.Endpoint, RequestToIdentifiableObject, BadRequestWhateverError>(addExistingGameRequest);

        await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
        Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
    }

    //[Fact]
    //public async Task Adding_an_existing_game_should_return_it_and_persist_it()
    //{

    //    // arrange
    //    var gameRequest = new RequestToIdentifiableObject
    //    {
    //        Id = 321016
    //    };

    //    // act
    //    var (response, initialGame) = await Client.POSTAsync<AddBggGame.Endpoint, RequestToIdentifiableObject, CustomGame>(gameRequest);
    //    var (getResponse, retrievedGame) = await Client.GETAsync<GetGame.Endpoint, RequestToIdentifiableObject, CustomGame>(new RequestToIdentifiableObject { Id = initialGame.Id});

    //    // assert
    //    await response.EnsureSuccessAndDumpBodyIfNotAsync(_output);
    //    await getResponse.EnsureSuccessAndDumpBodyIfNotAsync(_output);
    //    Assert.NotNull(retrievedGame);
    //    var expected = new CustomGame
    //    {
    //        Id = initialGame.Id,
    //        UserId = UserId,
    //        YearPublished = 2022,
    //        Name = "The Thing: Norwegian Outpost",
    //        MaxPlayer = 8,
    //        MinPlayer = 4,
    //        Description = "This expansion for The Thing - The Boardgame allows you to experience the events that happened at Thule Station, the Norwegian station where it all began! As for the 1982 version, the expansion lets you relive the tensest moments from the story, transporting you into the cinematic film!&#10;&#10;The core element of the game is based on the emulation properties of the Thing, which will hide its identity under a blanket of fake humanity, but with substantial differences in the way they interact with the game and with your feelings.&#10;&#10;Here are the key points of the expansion:&#10;&#10;     A new board, new characters, and new materials.&#10;     A new end game condition represented by the escape of the dog that will arrive at Outpost 31. IF this happens, you can play your next game of the 1982 version with a pack leader dog (a dog that behaves differently throughout the game).&#10;     A new element in the game: teeth! The Thule Station crew discovers that the Alien cannot assimilate and duplicate metal parts, so dental fillings become a way to find out if a person is definitely human, but not if they are alien. The test with the wire and the flamethrower disappears and flashlights are added with which it will be possible to carry out a test and look at the teeth of the other characters, checking for the presence or lack of fillings.&#10;     A new location is available for the Thing: The UFO! It represents the last hope of escape for the Thing, as in the movie. The final confrontation with any remaining humans will take place here. Of course, this is only an option, and just like the dog escape mentioned above, it doesn't necessarily happen! Everything will depend on your behavior, as you can win or lose in many ways!&#10;&#10;&#10;In the 2011 film, as soon as they found out what's going on, people's behavior becomes more selfish, trying by all means to save their own lives. We therefore decided to try to include the possibility of a solitary escape, an element that will bring the level of distrust to the maximum! This also brings new mechanics, such as the ability to voluntarily sabotage vehicles even if you are human.&#10; The expansion will contain: a new board, new characters, new cards and new tokens.&#10;&#10;&mdash;description from the publisher&#10;&#10;",
    //        MinAge = 14,
    //        DurationMinute = 90,
    //        IsExpansion = true,
    //        ImageUrl = "https://cf.geekdo-images.com/qPm9P2QfrMTRYk7wa7M5wA__original/img/zpCSSDVbpiowH_4oHFmxz5Qwd88=/0x0/filters:format(png)/pic5726092.png",
    //        ThumbnailUrl = "https://cf.geekdo-images.com/qPm9P2QfrMTRYk7wa7M5wA__thumb/img/2vd7G2hTXnwZM0OyFqJKXXGU-NQ=/fit-in/200x150/filters:strip_icc()/pic5726092.png"
    //    };

    //    Assert.Equivalent(expected, retrievedGame);
    //}
}
