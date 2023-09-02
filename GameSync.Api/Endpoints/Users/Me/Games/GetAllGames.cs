using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Business.BoardGameGeek.Model;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public static class GetAllGames
{

    public class Endpoint : EndpointWithoutRequest<IEnumerable<GamePreview>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get(string.Empty);
            Group<CollectionGroup>();
        }

        public override async Task<IEnumerable<GamePreview>> ExecuteAsync(CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);
            return await _context.Games
                .AsNoTracking()
                .Where(game => game.UserId == userId)
                .Select(game => new GamePreview
                {
                    Id = game.Id,
                    ImageUrl = game.ImageUrl,
                    IsExpansion = game.IsExpansion,
                    Name = game.Name,
                    ThumbnailUrl = game.ThumbnailUrl,
                    YearPublished  = game.YearPublished
                })
                .ToListAsync(cancellationToken: ct);
        }
    }

}
