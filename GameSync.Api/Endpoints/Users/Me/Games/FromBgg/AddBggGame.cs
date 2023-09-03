using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities.Games;
using GameSync.Business.BoardGamesGeek;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games.FromBgg;

public static class AddBggGame
{


    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<NotFound, Ok, BadRequestWhateverError>>
    {
        private readonly BoardGameGeekClient _client;
        private readonly GameSyncContext _context;

        public Endpoint(BoardGameGeekClient client, GameSyncContext context)
        {
            _client = client;
            _context = context;
        }

        public override void Configure()
        {
            Post("from-bgg/{Id}");
            Group<CollectionGroup>();
        }

        public override async Task<Results<NotFound, Ok, BadRequestWhateverError>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {
            var ids = new List<int> { req.Id };
            var game = (await _client.GetBoardGamesDetailAsync(ids)).FirstOrDefault();
            if (game is null)
            {
                return TypedResults.NotFound();
            }

            var userId = User.ClaimValue(ClaimsTypes.UserId);

            // check if the game has already been added 
            var gameExists = await _context.UserBoardGameGeekGames
                .AnyAsync(g => g.UserId == userId && g.BoardGameGeekGameId == req.Id);

            if (gameExists)
            {
                AddError(Resources.Resource.GameAlreadyAdded, nameof(Resources.Resource.GameAlreadyAdded));
                return new BadRequestWhateverError(ValidationFailures);
            }
            
            // check if the game has already been added in the db
            var gameAlreadyAdded = await _context.BoardGameGeekGames.AnyAsync(g => g.Id == req.Id);
            if (!gameAlreadyAdded)
            {
                var entityGame = new BoardGameGeekGame
                {
                    Id = req.Id,
                    Description = game.Description,
                    MaxPlayer = game.MaxPlayer.GetValueOrDefault(),
                    Name = game.Name ?? string.Empty,
                    MinAge = game.MinAge.GetValueOrDefault(),
                    MinPlayer = game.MinPlayer.GetValueOrDefault(),
                    DurationMinute = game.DurationMinute,
                    ImageUrl = game.ImageUrl,
                    ThumbnailUrl = game.ThumbnailUrl,
                    YearPublished = game.YearPublished ?? 0,
                    IsExpansion = game.IsExpansion,
                };
                await _context.BoardGameGeekGames.AddAsync(entityGame);
                await _context.SaveChangesAsync();
            }

            await _context.UserBoardGameGeekGames.AddAsync(new UserBoardGameGeekGame
            {
                BoardGameGeekGameId = req.Id,
                UserId = userId
            });
            await _context.SaveChangesAsync();

            return TypedResults.Ok();
        }
    }

}
