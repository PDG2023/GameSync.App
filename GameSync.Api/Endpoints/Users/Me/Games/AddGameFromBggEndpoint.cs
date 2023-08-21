﻿using FluentValidation;
using GameSync.Api.Common;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Business.BoardGamesGeek;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;


public class AddGameFromBggEndpoint : Endpoint<SingleGameRequest, Results<NotFound<IEnumerable<int>>, Ok, BadRequestWhateverError>>
{
    private readonly BoardGameGeekClient _client;
    private readonly GameSyncContext _context;

    public AddGameFromBggEndpoint(BoardGameGeekClient client, GameSyncContext context)
    {
        _client = client;
        _context = context;
    }

    public override void Configure()
    {
        Post("from-bgg/{Id}");
        Group<CollectionGroup>();
    }

    public override async Task<Results<NotFound<IEnumerable<int>>, Ok, BadRequestWhateverError>> ExecuteAsync(SingleGameRequest req, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            return new BadRequestWhateverError(ValidationFailures);
        }
        var ids = new List<int> { req.Id };
        var games = await _client.GetBoardGamesDetailAsync(ids);
        var game = games.FirstOrDefault();
        if (game is null)
        {
            return TypedResults.NotFound(ids.AsEnumerable());
        }

        var userId = User.ClaimValue(ClaimsTypes.UserId);
        // check if the game has already been added 
        var existingGame = await _context.BoardGameGeekGames.Where(g => g.UserId == userId && g.BoardGameGeekId == req.Id).FirstOrDefaultAsync();
        if (existingGame is not null)
        {
            AddError(Resources.Resource.GameAlreadyAdded, nameof(Resources.Resource.GameAlreadyAdded));
            return new BadRequestWhateverError(ValidationFailures);
        }

        var entityGame = new BoardGameGeekGame
        {
            BoardGameGeekId = req.Id,
            Description = game.Description,
            MaxPlayer = game.MaxPlayer.GetValueOrDefault(),
            Name = game.Name ?? string.Empty,
            MinAge = game.MinAge.GetValueOrDefault(),
            MinPlayer = game.MinPlayer.GetValueOrDefault(),
            DurationMinute = game.DurationMinute,
            UserId = userId!
        };
        
        await _context.BoardGameGeekGames.AddAsync(entityGame);
        await _context.SaveChangesAsync();

        return TypedResults.Ok();
    }
}
