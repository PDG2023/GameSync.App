using GameSync.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Endpoints.Users.Me.Parties;

public class PartyPreview
{
    public required int Id { get; init; }
    public string? Location { get; init; }
    public required string Name { get; init; }
    public required int NumberOfGames { get; init; }
    public DateTime DateTime { get; init; }
}

public class GetAllPartiesEndpoint : EndpointWithoutRequest<IEnumerable<PartyPreview>>
{
    private readonly GameSyncContext _context;

    public GetAllPartiesEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get(string.Empty);
        Group<PartiesGroup>();
    }

    public override async Task<IEnumerable<PartyPreview>> ExecuteAsync(CancellationToken ct)
    {

        var userId = User.ClaimValue(ClaimsTypes.UserId);

        var previews = await _context.Parties
            .Where(party => party.UserId == userId)
            .Select(party => new PartyPreview
            {
                Id = party.Id,
                Name = party.Name,
                NumberOfGames = party.Games == null ? 0 : party.Games.Count(),
                DateTime = party.DateTime,
                Location = party.Location
            })
            .ToListAsync();


        return previews;
    }
}
