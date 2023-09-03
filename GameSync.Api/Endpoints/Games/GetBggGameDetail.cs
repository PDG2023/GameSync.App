using GameSync.Api.BoardGameGeek;
using GameSync.Api.CommonRequests;
using GameSync.Api.CommonResponses;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GameSync.Api.Endpoints.Games;

public static class GetBggGameDetail
{
    public class Response
    {
        public required GameDetail GameDetail { get; init; }
        public required bool InCollection { get; init; }
    }

    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<Ok<Response>, NotFound, BadRequestWhateverError>>
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
            Get("{Id}");
            Group<GamesGroup>();
        }

        public override async Task<Results<Ok<Response>, NotFound, BadRequestWhateverError>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {

            var fetchedGames = await _client.GetBoardGamesDetailAsync(new[] { req.Id });
            var game = fetchedGames.FirstOrDefault();
            if (game is null)
            {
                return TypedResults.NotFound();
            }
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            return TypedResults.Ok(new Response
            {
                GameDetail = game,
                InCollection = userId is null || await _context.UserBoardGameGeekGames.AnyAsync(bgg => bgg.BoardGameGeekGameId == req.Id && bgg.UserId == userId)
            });
        }
    }
}
