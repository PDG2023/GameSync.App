using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty;

public class GetPartyDetails
{

    public class GameVoteInfo
    {
        public string? GameImageUrl { get; init; }
        public string? GameName { get; init; }
        public IEnumerable<string>? WhoVotedYes { get; init; }
        public int CountVotedYes => WhoVotedYes?.Count() ?? 0;
        public IEnumerable<string>? WhoVotedNo { get; init; }
        public int CountVotedNo => WhoVotedNo?.Count() ?? 0;

    }
    public class Response
    {
        public required string Name { get; init; }
        public required DateTime DateTime { get; init; }
        public string? Location { get; init; }

        public IEnumerable<GameVoteInfo>? GamesVoteInfo { get; init; }

    }

    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<Ok<Response>, NotFound>>
    {
        private readonly GameSyncContext _ctx;

        public Endpoint(GameSyncContext context)
        {
            _ctx = context;
        }

        public override void Configure()
        {
            Get("{Id}");
            Group<PartiesGroup>();

        }

        public override async Task<Results<Ok<Response>, NotFound>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);


            // TODO : This is extremely inefficient. Refactor, maybe using a raw sql or something.

            var partyDetails = _ctx.Parties
                .Where(p => p.UserId == userId && p.Id == req.Id)
                .Select(p => new Party
                {
                    DateTime = p.DateTime,
                    Location = p.Location,
                    Name = p.Name,
                    Games = p.Games == null ? null : p.Games.Select(g => new PartyGame
                    {
                        Game = new Game
                        {
                            ImageUrl = g.Game.ImageUrl,
                            Name = g.Game.Name
                        },

                        Votes = g.Votes == null ? null : g.Votes.Select(x => new Vote
                        {
                            UserName = x.UserId == null ? x.UserName : x.User.UserName,
                            VoteYes = x.VoteYes
                        }).ToArray()

                    }).ToArray()
                });

            var res = await partyDetails.FirstOrDefaultAsync();
            if (res is null)
            {
                return TypedResults.NotFound();
            }

            var result = new Response
            {
                DateTime = res.DateTime,
                Name = res.Name,
                Location = res.Location,
                GamesVoteInfo = res.Games?.Select(pg => new GameVoteInfo
                {
                    GameImageUrl = pg.Game.ImageUrl,
                    GameName = pg.Game.Name,
                    WhoVotedNo = pg.Votes?.Where(v => v.VoteYes is false).Select(v => v.UserName!),
                    WhoVotedYes = pg.Votes?.Where(v => v.VoteYes is true).Select(v => v.UserName!)
                })
            };

            return TypedResults.Ok(result);
        }
    }
}
