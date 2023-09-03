using GameSync.Api.CommonRequests;
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
        public required int Id { get; set; }
        public string? InvitationToken { get; set; }

    }

    public class Response
    {
        public required string Name { get; init; }
        public required DateTime DateTime { get; init; }
        public string? Location { get; init; }

        public IEnumerable<GameVoteInfo>? GamesVoteInfo { get; init; }

        public class GameVoteInfo
        {
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

            if (userId is null && req.InvitationToken is null)
            {
                return TypedResults.NotFound();
            }

            // TODO : This is extremely inefficient as the EF core query builder makes a lot a shit.
            //        Refactor, maybe using a raw sql or something.

            var partyDetails = _ctx.Parties
                .Where(p => p.Id == req.Id && (p.UserId == userId || p.InvitationToken == req.InvitationToken))
                .Select(p => new Response
                {
                    DateTime = p.DateTime,
                    Location = p.Location,
                    Name = p.Name,
                    GamesVoteInfo = p.Games == null ? null : p.Games.Select(g => new Response.GameVoteInfo
                    {

                        GameImageUrl = g.Game is UserBoardGameGeekGame ? ((UserBoardGameGeekGame)g.Game).BoardGameGeekGame.ImageUrl : ((CustomGame)g.Game).ImageUrl,
                        GameName = g.Game is UserBoardGameGeekGame ? ((UserBoardGameGeekGame)g.Game).BoardGameGeekGame.Name : ((CustomGame)g.Game).Name,
                        WhoVotedNo = g.Votes == null ? null : g.Votes.Where(g => g.VoteYes == false).Select(v => v.UserId == null ? v.UserName : v.User.UserName).ToArray(),
                        WhoVotedYes = g.Votes == null ? null : g.Votes.Where(g => g.VoteYes == true).Select(v => v.UserId == null ? v.UserName : v.User.UserName).ToArray(),
                    }).ToArray()
                });

            var res = await partyDetails.FirstOrDefaultAsync();
            if (res is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(res);
        }
    }
}
