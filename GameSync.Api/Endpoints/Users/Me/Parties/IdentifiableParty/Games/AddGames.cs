using FluentValidation;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;

public static class AddGames
{

    public class Request
    {
        public int PartyId { get; init; }

        public IEnumerable<PartyGameInfo> Games { get; init; } = Array.Empty<PartyGameInfo>();

        public class PartyGameInfo
        {
            public required int Id { get; set; }
            public required bool IsCustom { get; set; }
        }

    }


    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.PartyId).GreaterThan(0);
        }
    }

    public class Endpoint : Endpoint<Request, Results<NotFound, Ok, BadRequestWhateverError>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Post("{PartyId}/games");
            Group<PartiesGroup>();
            DontAutoTag();
            Options(builder => builder.WithTags("Party's games"));
        }

        public override async Task<Results<NotFound, Ok, BadRequestWhateverError>> ExecuteAsync(Request req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var party = await _context.Parties.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == req.PartyId);
            if (party is null)
            {
                return TypedResults.NotFound();
            }

            var customGamesIds = req.Games.Where(g => g.IsCustom).Select(r => r.Id).ToList();
            var bggGamesIds = req.Games.Where(g => !g.IsCustom).Select(r => r.Id).ToList();

            if (bggGamesIds.Any())
            {
                var bggGamesInCollection = await _context.UserBoardGameGeekGames
                    .Where(g => g.UserId == userId && bggGamesIds.Contains(g.BoardGameGeekGameId))
                    .CountAsync();

                if (bggGamesInCollection != bggGamesIds.Count)
                {
                    return TypedResults.NotFound();
                }
            }

            if (customGamesIds.Any())
            {
                var customGamesInCollection = await _context.CustomGames
                    .Where(g => g.UserId == userId && customGamesIds.Contains(g.Id))
                    .CountAsync();

                if (customGamesInCollection != customGamesIds.Count)
                {
                    return TypedResults.NotFound();
                }
            }


            try
            {
                foreach (var customGameId in customGamesIds)
                {
                    await _context.PartyCustomGames.AddAsync(new PartyCustomGame
                    {
                        GameId = customGameId,
                        PartyId = party.Id,
                    });
                }


                foreach (var bggGameId in bggGamesIds)
                {
                    await _context.PartyBoardGameGeekGames.AddAsync(new PartyBoardGameGeekGame
                    {
                        BoardGameGeekId = bggGameId,
                        PartyId = party.Id,
                    });
                }

                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is PostgresException npgException
                    && npgException.SqlState == PostgresErrorCodes.UniqueViolation)
                {
                    AddError(nameof(Resources.Resource.GameAlreadyAdded), Resources.Resource.GameAlreadyAdded);
                    return new BadRequestWhateverError(ValidationFailures);
                }
            }

            return TypedResults.Ok();
        }
    }
}
