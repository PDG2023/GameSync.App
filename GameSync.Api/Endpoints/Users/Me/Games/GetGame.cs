using GameSync.Api.CommonRequests;
using GameSync.Api.CommonResponses;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public static class GetGame
{

    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<Ok<GameDetail>, NotFound>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("{Id}");
            Group<CollectionGroup>();
        }

        public override async Task<Results<Ok<GameDetail>, NotFound>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {

            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var game = await _context.CustomGames
                .FirstOrDefaultAsync(g => g.UserId == userId && g.Id == req.Id);

            if (game is null)
            {
                return TypedResults.NotFound();
            }

            var response = new GameDetail 
            { 
                Id = req.Id,
                ImageUrl = game.ImageUrl,
                Name = game.Name,
                ThumbnailUrl = game.ThumbnailUrl,
                YearPublished = game.YearPublished,
                Description = game.Description,
                DurationMinute = game.DurationMinute,
                IsExpansion = game.IsExpansion,
                MaxPlayer  = game.MaxPlayer,
                MinAge = game.MinAge,
                MinPlayer = game.MinPlayer
            };

            return TypedResults.Ok(response);
        }
    }

}
