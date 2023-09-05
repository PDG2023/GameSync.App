using FluentValidation;
using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public static class DeleteGame
{

    public class Request : RequestToIdentifiableObject
    {
        public required bool IsCustomGame { get; set;}
    }

    public class Endpoint : Endpoint<Request, Results<BadRequestWhateverError, NotFound, Ok>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Delete("{Id}");
            Group<CollectionGroup>();
        }

        public override async Task<Results<BadRequestWhateverError, NotFound, Ok>> ExecuteAsync(Request req, CancellationToken ct)
        {

            var userId = User.ClaimValue(ClaimsTypes.UserId);
            int count;

            if (req.IsCustomGame)
            {
                count = await _context.CustomGames.Where(game => game.Id == req.Id && game.UserId == userId).ExecuteDeleteAsync();
            }
            else
            {
                count = await _context.UserBoardGameGeekGames.Where(game => game.BoardGameGeekGameId == req.Id && game.UserId == userId).ExecuteDeleteAsync();
            }

            if (count == 0)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok();

        }



    }

}
