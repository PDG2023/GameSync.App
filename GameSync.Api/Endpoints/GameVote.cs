using FluentValidation;
using GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints;

public static class GameVote
{

    public class Request
    {
        public int GameId { get; init; }

        public int? PartyId { get; init; }
        public string? InvitationToken { get; init; }
        public bool? VoteYes { get; init; }

        public string? UserName { get; init; }

    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.GameId).GreaterThan(0);
            RuleFor(r => r.PartyId).GreaterThan(0).When(r => r.PartyId is not null);
        }
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
            Put("users/me/parties/{PartyId}/games/{GameId}/vote", "parties/{InvitationToken}/games/{GameId}/vote");
            DontAutoTag();
            Options(x => x.WithTags("Vote"));
            AllowAnonymous();
        }


        public override async Task<Results<Ok, NotFound, BadRequest>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            if (userId is null)
            {
                if (req.InvitationToken is null)
                    return TypedResults.NotFound();

                if (req.UserName is null)
                    return TypedResults.BadRequest();
            }

            var partyGameSearch = _ctx.PartiesGames
                .Where(pg => pg.GameId == req.GameId 
                            && (pg.Party.UserId == userId || pg.Party.InvitationToken == req.InvitationToken));

            var partyGame = await partyGameSearch.FirstOrDefaultAsync();

            if (partyGame is null)
            {
                return TypedResults.NotFound();
            }

            Vote? voteOfUser;
            if (userId is null) // anonymous
            {
                voteOfUser = partyGame.Votes?.FirstOrDefault(v => v.UserName == req.UserName);
            }
            else
            {
                voteOfUser = partyGame.Votes?.FirstOrDefault(v => v.UserId == userId);
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
                if (voteOfUser.VoteYes == req.VoteYes)
                {
                    return TypedResults.Ok();
                }

                voteOfUser.VoteYes = req.VoteYes;
            }

            await _ctx.SaveChangesAsync();

            return TypedResults.Ok();
        }

    }
}
