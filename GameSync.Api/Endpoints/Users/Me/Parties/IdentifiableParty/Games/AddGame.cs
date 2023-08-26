using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;

public class AddGame
{


    public class Endpoint : Endpoint<PartyGameRequest, Results<NotFound, Ok>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            this._context = context;
        }

        public override void Configure()
        {
            Put("{PartyId}/games/{GameId}");
            Group<PartiesGroup>();
        }

        public override Task<Results<NotFound, Ok>> ExecuteAsync(PartyGameRequest req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);



            return base.ExecuteAsync(req, ct);
        }

    }
}
