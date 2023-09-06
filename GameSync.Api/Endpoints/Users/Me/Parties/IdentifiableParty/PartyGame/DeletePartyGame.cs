using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.PartyGame;

public static class DeletePartyGame
{
    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<Ok, NotFound>>
    {
        private readonly GameSyncContext _ctx;

        public Endpoint(GameSyncContext ctx)
        {
            _ctx = ctx;
        }

        public override void Configure()
        {
            Delete("{PartyId}/games/{Id}");
            DontAutoTag();
            Options(builder => builder.WithTags("Party's games"));
            Group<PartiesGroup>();
        }

        public override async Task<Results<Ok, NotFound>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var numberDeletedRows = await _ctx.PartiesGames
                .Where(g => g.Id == req.Id && g.Party.UserId == userId)
                .ExecuteDeleteAsync(ct);

            if (numberDeletedRows == 0)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok();
        }

    }
}

