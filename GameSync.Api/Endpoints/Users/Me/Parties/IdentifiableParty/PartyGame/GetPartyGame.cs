using GameSync.Api.CommonResponses;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.PartyGame;

public static class GetPartyGame
{

    public class Request
    {
        public string? InvitationToken { get; init; }
        public int? PartyId { get; init; }
        public required int PartyGameId { get; init; }

    }

    public class Response
    {
        public required GameDetail Game { get; init; }
        public bool IsCustom { get; init; }
    }

    public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Get("users/me/parties/{PartyId}/games/{PartyGameId}", "parties/{InvitationToken}/games/{PartyGameId}");
            DontAutoTag();
            Options(builder => builder.WithTags("Party's games"));
            AllowAnonymous();
        }

        public override async Task<Results<Ok<Response>, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            if (userId is null && req.InvitationToken is null)
            {
                return TypedResults.NotFound();
            }

            var partyGame = await _context.PartiesGames
                .Where(pg => pg.Id == req.PartyGameId)
                .Where(pg => pg.Party.InvitationToken != null && pg.Party.InvitationToken == req.InvitationToken || pg.Party.UserId == userId && pg.Party.Id == req.PartyId)
                .Select(pg => new Response
                {
                    Game = new GameDetail(pg is PartyBoardGameGeekGame ? (pg as PartyBoardGameGeekGame)!.BoardGameGeekGame : (pg as PartyCustomGame)!.Game),
                    IsCustom = pg is PartyCustomGame
                })
                .FirstOrDefaultAsync();

            if (partyGame is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(partyGame);
        }

    }
}
