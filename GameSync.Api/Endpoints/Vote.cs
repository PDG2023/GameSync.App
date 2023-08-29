using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace GameSync.Api.Endpoints;

public static class GameVote
{

    public class Request : PartyGameRequest
    {
        public bool? VoteYes { get; init; }

        public string? UserName { get; init; }

    }

    public class Endpoint : Endpoint<Request, Results<Ok, NotFound>>
    {
        private readonly GameSyncContext _ctx;

        public Endpoint(GameSyncContext ctx)
        {
            _ctx = ctx;
        }

        public override void Configure()
        {
            Put("parties/{PartyId}/games/{GameId}/vote");
            DontAutoTag();
            Options(x =>
            {
                x.WithTags("Votes");
            });
            AllowAnonymous();
        }


        public override async Task<Results<Ok, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var partyGame = _ctx.PartiesGames.FirstOrDefaultAsync(pg => pg.PartyId == req.PartyId && pg.GameId == req.GameId);

            if (partyGame is null)
            {
                return TypedResults.NotFound();
            }

            if (userId is null) // anonymous
            {
                var userVote = _ctx.PartiesGames
                    
                    .Where(pg => pg.Votes.Any(x => x.UserName == req.UserName))
                    .SelectMany(pg => pg.Votes);
                if (await userVote.AnyAsync())
                {
                    await userVote.ExecuteUpdateAsync(builder =>
                    {
                        builder.SetProperty(v => v.)
                    })
                }
            }
            else
            {
                _ctx.
            }
            
            return base.ExecuteAsync(req, ct);
        }

    }
}
