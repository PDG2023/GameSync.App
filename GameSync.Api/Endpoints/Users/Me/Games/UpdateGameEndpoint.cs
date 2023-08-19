using FastEndpoints.Security;
using FluentValidation;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class UpdateGameRequest : IGame
{
    public required int GameId { get; init; }

    public string? Name { get; init; }
    public int? MinPlayer { get; init; }
    public int? MaxPlayer { get; init; }
    public int? MinAge { get; init; }
    public string? Description { get; init; }
    public int? DurationMinutes { get; init; }
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
        var userId = User.ClaimValue(ClaimsTypes.UserId);

        var game = await _context.Games.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == req.GameId);
        if (game is null)
        {
            return TypedResults.NotFound();
        }

        if (req.MaxPlayer is not null)
        {
            game.MaxPlayer = req.MaxPlayer.Value;
        }
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
        
        return TypedResults.Ok(game);
            //Expression<Func<SetPropertyCalls<Game>, SetPropertyCalls<Game>>> fieldsToUpdate = calls => calls;

        
            //if (req.MaxPlayer is not null)
            //{
            //    fieldsToUpdate = AppendColumnToUpdate(fieldsToUpdate, s => s.SetProperty(game => game.MaxPlayer, req.MaxPlayer));
            //}


            //var colUpdated = await _context.Games.Where(game => game.Id == req.GameId && game.UserId == userId)
            //    .ExecuteUpdateAsync(fieldsToUpdate);

            //if (colUpdated == 0)
            //{
            //    return TypedResults.NotFound();
            //}



    }


    private Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> AppendColumnToUpdate<TEntity>(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> set,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> newColumn)
    {
        var replace = new ReplacingExpressionVisitor(set.Parameters, new[] { set.Body });
        var combined = replace.Visit(newColumn.Body);
        return Expression.Lambda<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>(combined, set.Parameters);
    }
}
