using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence.Entities.Games;
using GameSync.Business.BoardGameGeek.Model;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public static class GetAllGames
{

    public class Endpoint : EndpointWithoutRequest<IEnumerable<GameCollectionItem>>
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

        public override async Task<IEnumerable<GameCollectionItem>> ExecuteAsync(CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);
            return await _context.Games
                .AsNoTracking()
                .Where(g => g.UserId == userId)
                .Include(g => ((UserBoardGameGeekGame)g).BoardGameGeekGame)
                .Select(g => g is UserBoardGameGeekGame ? ToItem((UserBoardGameGeekGame)g) : ToItem((CustomGame)g))
                .ToListAsync();
        }

        private static GameCollectionItem ToItem(UserBoardGameGeekGame bgg)
        {
            return new GameCollectionItem
            {
                Id = bgg.BoardGameGeekGameId,
                ImageUrl = bgg.BoardGameGeekGame.ImageUrl,
                Name = bgg.BoardGameGeekGame.Name,
                ThumbnailUrl = bgg.BoardGameGeekGame.ThumbnailUrl,
                YearPublished = bgg.BoardGameGeekGame.YearPublished,
                IsExpansion = bgg.BoardGameGeekGame.IsExpansion,
                IsCustom = false

            };
        }

        private static GameCollectionItem ToItem(CustomGame game)
        {
            return new GameCollectionItem
            {
                Id = game.Id,
                ImageUrl = game.ImageUrl,
                Name = game.Name,
                ThumbnailUrl = game.ThumbnailUrl,
                YearPublished = game.YearPublished,
                IsExpansion = game.IsExpansion,
                IsCustom = true
            };
        }

    }

}
