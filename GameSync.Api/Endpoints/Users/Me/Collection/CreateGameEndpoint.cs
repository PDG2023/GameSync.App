using FluentValidation;
using GameSync.Api.Persistence;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
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

public class CreateGameValidator : Validator<CreateGameRequest>
{
    public CreateGameValidator() 
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.MinPlayer).GreaterThan(0);
        RuleFor(x => x.MaxPlayer).GreaterThan(0);
        RuleFor(x => x.MinAge).GreaterThan(0).LessThan(120);
        RuleFor(x => x.DurationMinute).GreaterThan(0);
        RuleFor(x => x.Description).MaximumLength(500);
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
        DontThrowIfValidationFails();
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

