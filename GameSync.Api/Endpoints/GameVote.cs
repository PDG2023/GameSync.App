using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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

            var partyGameSearch = _ctx.PartiesGames.Where(pg => pg.PartyId == req.PartyId && pg.GameId == req.GameId);

            var partyGame = await partyGameSearch.FirstOrDefaultAsync();

            if (partyGame is null)
            {
                return TypedResults.NotFound();
            }

            if (userId is null) // anonymous
            {
                var voteOfUser = partyGame.Votes?.FirstOrDefault(v => v.UserName == req.UserName);

                if (voteOfUser is null)
                {
                    var vote = new Vote { UserName = req.UserName, VoteYes = req.VoteYes };
                    if (partyGame.Votes is null)
                    {
                        partyGame.Votes = new List<Vote> { vote };
                    }
                    else
                    {
                        partyGame.Votes.Add(vote);
                    }
                }
                else
                {
                    voteOfUser.VoteYes = req.VoteYes;
                }

                await _ctx.SaveChangesAsync();
            }
            else
            {
                // TODO
            }
            
            return TypedResults.Ok();
        }

    }
}
