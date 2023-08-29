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

    public class Endpoint : Endpoint<Request, Results<Ok, NotFound, BadRequest>>
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


        public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var partyGameSearch = _ctx.PartiesGames.Where(pg => pg.PartyId == req.PartyId && pg.GameId == req.GameId);

            var partyGame = await partyGameSearch.FirstOrDefaultAsync();

            if (partyGame is null)
            {
                return TypedResults.NotFound();
            }

            Vote? voteOfUser;
            if (userId is null) // anonymous
            {
                if (req.UserName is null)
                {
                    return TypedResults.BadRequest();
                }

                voteOfUser = partyGame.Votes?.FirstOrDefault(v => v.UserName == req.UserName);
            }
            else
            {
                voteOfUser = partyGame.Votes?.FirstOrDefault(v => v.UserName == req.UserName);
            }

            if (voteOfUser is null)
            {
                var vote = new Vote { VoteYes = req.VoteYes };

                if (userId is null)
                {
                    vote.UserName = req.UserName;
                }
                else
                {
                    vote.UserId = userId;
                }
                partyGame.Votes.Add(vote);
            }
            else
            {
                voteOfUser.VoteYes = req.VoteYes;
            }

            await _ctx.SaveChangesAsync();

            return TypedResults.Ok();
        }

    }
}
