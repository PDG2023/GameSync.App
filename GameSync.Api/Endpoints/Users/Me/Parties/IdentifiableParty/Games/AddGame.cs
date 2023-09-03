using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GameSync.Api.Endpoints.Users.Me.Parties.IdentifiableParty.Games;

public static class AddGame
{

    public class Endpoint : Endpoint<PartyGameRequest, Results<NotFound, Ok, BadRequestWhateverError>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Put(string.Empty);
            Group<PartyGameGroup>();
        }

        public override async Task<Results<NotFound, Ok, BadRequestWhateverError>> ExecuteAsync(PartyGameRequest req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);

            var userGame = await _context.CustomGames
                .FirstOrDefaultAsync(g => g.Id == req.GameId && g.UserId == userId);

            if (userGame is null)
            {
                return TypedResults.NotFound();
            }

            try
            {

                var partyGame = await _context.AddAsync(new PartyGame
                {
                    GameId = req.GameId,
                    PartyId = req.PartyId
                });
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
