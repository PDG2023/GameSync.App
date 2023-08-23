using GameSync.Api.CommonRequests;
using GameSync.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Parties;

public static class DeleteParty
{

    public class Endpoint : Endpoint<RequestToIdentifiableObject, Results<NotFound, Ok>>
    {
        private readonly GameSyncContext _context;

        public Endpoint(GameSyncContext context)
        {
            _context = context;
        }

        public override void Configure()
        {
            Delete("{Id}");
            Group<PartiesGroup>();
        }

        public override async Task<Results<NotFound, Ok>> ExecuteAsync(RequestToIdentifiableObject req, CancellationToken ct)
        {
            var userId = User.ClaimValue(ClaimsTypes.UserId);
            var partyExists = await _context.Parties
                .AnyAsync(party => party.Id == req.Id && userId == party.UserId);

            if (!partyExists)
            {
                return TypedResults.NotFound();
            }

            await _context.Parties
                .Where(party => party.Id == req.Id)
                .ExecuteDeleteAsync();

            return TypedResults.Ok();
        }

    }

}
