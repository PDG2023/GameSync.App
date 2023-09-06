using GameSync.Api.CommonRequests;
using GameSync.Api.Endpoints.Games;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty;

public static class GetParty
{

    public class Request
    {
        public int? Id { get; set; }
        public string? InvitationToken { get; set; }

    }

    public class Response
    {
        public required string Name { get; init; }
        public required DateTime DateTime { get; init; }
        public string? Location { get; init; }
        public required bool IsOwner { get; init; }

        public IEnumerable<PartyGameInfo>? GamesVoteInfo { get; init; }

        public class PartyGameInfo
        {
            public required int Id { get; set; }
            public string? GameImageUrl { get; init; }
            public string? GameName { get; init; }
            public IEnumerable<string>? WhoVotedYes { get; init; }
            public int CountVotedYes => WhoVotedYes?.Count() ?? 0;
            public IEnumerable<string>? WhoVotedNo { get; init; }
            public int CountVotedNo => WhoVotedNo?.Count() ?? 0;

        }


    }

    public class Endpoint : Endpoint<Request, Results<Ok<Response>, NotFound>>
    {
        private readonly GameSyncContext _ctx;

        public Endpoint(GameSyncContext context)
        {
            _ctx = context;
        }

        public override void Configure()
        {
            Get("users/me/parties/{Id}", "parties/{InvitationToken}");
            DontAutoTag();
            Options(x => x.WithTags("Parties"));
            AllowAnonymous();
        }

        public override async Task<Results<Ok<Response>, NotFound>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            if (userId is null )
            {
                if (req.Id is not null || req.InvitationToken is null)
                {
                    return TypedResults.NotFound();
                }
            }

            // TODO : This is extremely inefficient as the EF core query builder makes a lot a shit.
            //        Refactor, maybe using a raw sql or something.

            var partyDetails = _ctx.Parties
                .Where(p =>  req.Id != null 
                    ? req.Id == p.Id && p.UserId ==  userId
                    : p.InvitationToken == req.InvitationToken)
                .Select(p => new Response
                {
                    DateTime = p.DateTime,
                    Location = p.Location,
                    Name = p.Name,
                    IsOwner = userId != null && p.UserId == userId,
                    GamesVoteInfo = p.Games == null ? null : p.Games.Select(pg => new Response.PartyGameInfo
                    {
                        Id = pg.Id,
                        GameImageUrl = pg is PartyBoardGameGeekGame ? ((PartyBoardGameGeekGame)pg).BoardGameGeekGame.ImageUrl : ((PartyCustomGame)pg).Game.ImageUrl,
                        GameName = pg is PartyBoardGameGeekGame ? ((PartyBoardGameGeekGame)pg).BoardGameGeekGame.Name : ((PartyCustomGame)pg).Game.Name,
                        WhoVotedNo = pg.Votes == null ? null : pg.Votes.Where(g => g.VoteYes == false).Select(v => v.UserId == null ? v.UserName : v.User.UserName).ToArray(),
                        WhoVotedYes = pg.Votes == null ? null : pg.Votes.Where(g => g.VoteYes == true).Select(v => v.UserId == null ? v.UserName : v.User.UserName).ToArray(),
                    }).ToArray()
                }).AsSplitQuery();

            var res = await partyDetails.FirstOrDefaultAsync();
            if (res is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(res);
        }

    }
}
