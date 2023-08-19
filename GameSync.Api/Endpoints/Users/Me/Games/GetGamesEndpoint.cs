using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class GetGamesEndpoint : EndpointWithoutRequest<IEnumerable<Game>>
{
    private readonly GameSyncContext _context;

    public GetGamesEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get(string.Empty);
        Group<CollectionGroup>();
    }

    public override async Task<IEnumerable<Game>> ExecuteAsync(CancellationToken ct)
    {
        var userId = User.ClaimValue(ClaimsTypes.UserId);
        return await _context.Games.Where(game => game.UserId == userId).ToListAsync(cancellationToken: ct);
    }
}
