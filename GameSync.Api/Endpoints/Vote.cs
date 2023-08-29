using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameSync.Api.Endpoints;

public static class GameVote
{

    public class Request : PartyGameRequest
    {
        public bool? VoteYes { get; set; }
    }

    [HttpPut("parties/{PartyId}/games/{GameId}/vote")]
    public class Endpoint : Endpoint<PartyGameRequest, Results<Ok, NotFound>>
    {
        private readonly GameSyncContext _ctx;

        public Endpoint(GameSyncContext ctx)
        {
            _ctx = ctx;
        }

        public override Task<Results<Ok, NotFound>> ExecuteAsync(PartyGameRequest req, CancellationToken ct)
        {
            return base.ExecuteAsync(req, ct);
        }

    }
}
