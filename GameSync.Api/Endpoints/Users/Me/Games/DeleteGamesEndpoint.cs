using Duende.IdentityServer.Models;
using FluentValidation;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class DeleteGamesRequest
{
    public required IList<int> GamesId { get; init; }
}

public class DeleteGamesValidator : Validator<DeleteGamesRequest>
{
    public DeleteGamesValidator()
    {
        RuleForEach(req => req.GamesId).GreaterThan(0);
    }
}

public class DeleteGamesEndpoint : Endpoint<DeleteGamesRequest, Results<BadRequestWhateverError, NotFound<List<int>>, Ok>>
{
    private readonly GameSyncContext _context;

    public DeleteGamesEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete(string.Empty);
        Group<CollectionGroup>();
    }

    public override async Task<Results<BadRequestWhateverError, NotFound<List<int>>, Ok>> ExecuteAsync(DeleteGamesRequest req, CancellationToken ct)
    {

        if (ValidationFailed)
        {
            await Task.CompletedTask;
            return new BadRequestWhateverError(ValidationFailures);
        }

        var userId = User.ClaimValue(ClaimsTypes.UserId);
        var requestedGamesIds = await _context.Games
            .AsNoTracking()
            .Where(game => req.GamesId.Contains(game.Id) && game.UserId == userId)
            .Select(g => g.Id)
            .ToListAsync(ct);

        if (req.GamesId.Count != requestedGamesIds.Count)
        {
            var idsNotFound = req.GamesId.Except(requestedGamesIds).ToList();
            return TypedResults.NotFound(idsNotFound);

        }

        await _context.Games.Where(game => requestedGamesIds.Contains(game.Id)).ExecuteDeleteAsync();

        return TypedResults.Ok();

    }



}
