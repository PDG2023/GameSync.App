using FluentValidation;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using GameSync.Business;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json.Serialization;

namespace GameSync.Api.Endpoints.Users.Me.Games;


public class CreateGameRequest : IGame
{
    public required string Name { get; init; }
    public required int MinPlayer { get; init; }
    public required int MaxPlayer { get; init; }
    public required int MinAge { get; init; }
    public string? Description { get; init; }
    public int? DurationMinute { get; init; }

    [JsonIgnore]
    int? IGame.MinPlayer => MinPlayer;

    [JsonIgnore]
    int? IGame.MaxPlayer => MaxPlayer;

    [JsonIgnore]
    int? IGame.MinAge => MinAge;
}


public class CreateGameValidator : Validator<CreateGameRequest>
{
    public CreateGameValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        Include(new GameValidator());
    }
}

public class CreateGameEndpoint : Endpoint<CreateGameRequest, Results<Ok<Game>, BadRequestWhateverError>>
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

    public override async Task<Results<Ok<Game>, BadRequestWhateverError>> ExecuteAsync(CreateGameRequest req, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            return new BadRequestWhateverError(ValidationFailures);
        }
        var trackingGame = await _context.Games.AddAsync(RequestToGame(req), ct);
        await _context.SaveChangesAsync(ct);
        return TypedResults.Ok(trackingGame.Entity);
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

