using GameSync.Api.Common;
using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class GetGameEndpoint : Endpoint<RequestToIdentifiableObject, Results<Ok<Game>, NotFound>>
{
    private readonly GameSyncContext _context;

    public GetGameEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("{Id}");
        Group<CollectionGroup>();
    }

    public override async Task<Results<Ok<Game>, NotFound>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
    {

        var userId = User.ClaimValue(ClaimsTypes.UserId);

        var game = await _context.Games
            .FirstOrDefaultAsync(g => g.UserId == userId && g.Id == req.Id);
            
        if (game is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(game);
    }
}
