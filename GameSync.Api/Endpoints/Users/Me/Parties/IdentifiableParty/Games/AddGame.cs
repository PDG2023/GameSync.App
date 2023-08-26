using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;

public class AddGame
{

    public class Endpoint : Endpoint<PartyGameRequest, Results<NotFound, Ok>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Put("{PartyId}/games/{GameId}");
            Group<PartiesGroup>();
        }

        public override async Task<Results<NotFound, Ok>> ExecuteAsync(PartyGameRequest req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var userGame = await _context.Games
                .FirstOrDefaultAsync(g => g.Id == req.GameId && g.UserId == userId);

            if (userGame is null)
            {
                return TypedResults.NotFound();
            }

            var partyGame = await _context.AddAsync(new PartyGame
            {
                GameId = req.GameId,
                PartyId = req.PartyId
            });
            await _context.SaveChangesAsync();

            return TypedResults.Ok();
        }
    }
}
