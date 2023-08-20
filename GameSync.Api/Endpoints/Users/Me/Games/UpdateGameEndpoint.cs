using FastEndpoints.Security;
using FluentValidation;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Resources;
using GameSync.Business;
using GameSync.Business.BoardGamesGeek.Schemas;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class UpdateGameRequest : IGame
{
    public required int GameId { get; init; }

    public string? Name { get; init; }
    public int? MinPlayer { get; init; }
    public int? MaxPlayer { get; init; }
    public int? MinAge { get; init; }
    public string? Description { get; init; }
    public int? DurationMinute { get; init; }
}

public class UpdateGameValidator : Validator<UpdateGameRequest>
{
    public UpdateGameValidator()
    {
        RuleFor(x => x.GameId).GreaterThan(0);
        Include(new GameValidator());
    }
}

public class UpdateGameEndpoint : Endpoint<UpdateGameRequest, Results<NotFound, Ok<Game>, BadRequestWhateverError>>
{
    private readonly GameSyncContext _context;

    public UpdateGameEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch(string.Empty);
        Group<CollectionGroup>();
    }

    public override async Task<Results<NotFound, Ok<Game>, BadRequestWhateverError>> ExecuteAsync(UpdateGameRequest req, CancellationToken ct)
    {

        if (ValidationFailed)
        {
            return new BadRequestWhateverError(ValidationFailures);
        }


        var userId = User.ClaimValue(ClaimsTypes.UserId);

        var game = await _context.Games.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == req.GameId);
        if (game is null)
        {
            return TypedResults.NotFound();
        }

        UpdateProperties(game, req);

        if (ValidationFailed)
        {
            return new BadRequestWhateverError(ValidationFailures);
        }

        _context.Games.Update(game);
        await _context.SaveChangesAsync();

        return TypedResults.Ok(game);

    }

    private void UpdateProperties(Game game, UpdateGameRequest req)
    {

        if (req.MinPlayer is not null)
        {
            if (req.MinPlayer > game.MaxPlayer)
            {
                AddError(x => x.MinPlayer, Resource.MaxPlayerLowerThanMinPlayer, nameof(Resource.MaxPlayerLowerThanMinPlayer));
            }
            else
            {
                game.MinPlayer = req.MinPlayer.Value;
            }
        }

        if (req.MaxPlayer is not null)
        {
            if (req.MaxPlayer < game.MinPlayer)
            {
                AddError(x => x.MaxPlayer, Resource.MaxPlayerLowerThanMinPlayer, nameof(Resource.MaxPlayerLowerThanMinPlayer));
            }
            else
            {
                game.MaxPlayer = req.MaxPlayer.Value;
            }
        }


        if (req.Name is not null)
        {
            game.Name = WebUtility.HtmlEncode(req.Name);
        }

        if (req.Description is not null)
        {
            game.Description = WebUtility.HtmlEncode(req.Description);
        }

        if (req.DurationMinute is not null)
        {
            game.DurationMinute = req.DurationMinute;
        }

        if (req.MinAge is not null)
        {
            game.MinAge = req.MinAge.Value;
        }

    }

}