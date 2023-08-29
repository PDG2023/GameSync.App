using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;

public static class DeleteGame
{
   public class Endpoint : Endpoint<PartyGameRequest, Results<Ok, NotFound>>
   {
        private readonly GameSyncContext _ctx;

        public Endpoint(GameSyncContext ctx) 
        {
            _ctx = ctx;
        }

        public override void Configure()
        {
            Delete(string.Empty);
            Group<PartyGameGroup>();
        }

        public override async Task<Results<Ok, NotFound>> ExecuteAsync(PartyGameRequest req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var numberDeletedRows = await _ctx.PartiesGames
                .Where(g => g.GameId == req.GameId
                         && g.PartyId == req.PartyId
                         && g.Game.UserId == userId)
                .ExecuteDeleteAsync(ct);

            if (numberDeletedRows == 0)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok();
        }

    }
}

