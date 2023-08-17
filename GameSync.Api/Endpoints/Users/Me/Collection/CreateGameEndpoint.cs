using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using System.Net;

namespace GameSync.Api.Endpoints.Users.Me.Collection;


public class CreateGameRequest
{
    public required string Name { get; init; }
    public required int MinPlayer { get; init; }
    public required int MaxPlayer { get; init; }
    public required int MinAge { get; init; }
    public string? Description { get; init; }
    public int? DurationMinute { get; init; }
}


public class CreateGameEndpoint : Endpoint<CreateGameRequest, Game>
{
    private readonly GameSyncContext _context;
    public CreateGameEndpoint(GameSyncContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post(string.Empty);
        Group<CollectionGroup>();
    }

    public override async Task<Game> ExecuteAsync(CreateGameRequest req, CancellationToken ct)
    {
        var trackingGame = await _context.Games.AddAsync(RequestToGame(req), ct);
        await _context.SaveChangesAsync(ct);
        return trackingGame.Entity;
    }

    public  Game RequestToGame(CreateGameRequest r)
    {
        return new Game
        {
            MaxPlayer = r.MaxPlayer,
            MinPlayer = r.MinPlayer,
            Name = WebUtility.HtmlEncode(r.Name),
            MinAge = r.MinAge,
            UserId = User.ClaimValue(ClaimsNames.UserId)!,
            Description = WebUtility.HtmlEncode(r.Description),
            DurationMinute = r.DurationMinute
        };
    }
}

