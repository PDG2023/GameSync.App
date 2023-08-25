using FluentValidation;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json.Serialization;

namespace GameSync.Api.Endpoints.Users.Me.Games;


public class CreateGameRequest : IGameRequest
{
    public required string Name { get; init; }
    public required int MinPlayer { get; init; }
    public required int MaxPlayer { get; init; }
    public required int MinAge { get; init; }
    public string? Description { get; init; }
    public int? DurationMinute { get; init; }

    [JsonIgnore]
    int? IGameRequest.MinPlayer => MinPlayer;

    [JsonIgnore]
    int? IGameRequest.MaxPlayer => MaxPlayer;

    [JsonIgnore]
    int? IGameRequest.MinAge => MinAge;
}


public class CreateGameRequestValidator : Validator<CreateGameRequest>
{
    public CreateGameRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        Include(new GameValidator());
    }
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

    private  Game RequestToGame(CreateGameRequest r)
    {
        return new Game
        {
            MaxPlayer = r.MaxPlayer,
            MinPlayer = r.MinPlayer,
            Name = WebUtility.HtmlEncode(r.Name),
            MinAge = r.MinAge,
            UserId = User.ClaimValue(ClaimsTypes.UserId)!,
            Description = WebUtility.HtmlEncode(r.Description),
            DurationMinute = r.DurationMinute
        };
    }
}

