using FluentValidation;
using GameSync.Api.Endpoints.Users.Me.Games;
using GameSync.Api.Persistence.Entities;

namespace GameSync.Api.Endpoints.Users.Me.Games;

public class UpdateGameRequest : IGame
{
    public required int GameId { get; init; }

    public string? Name { get; init; }
    public int? MinPlayer { get; init; }
    public int? MaxPlayer { get; init; }
    public int? MinAge { get; init; }
    public string? Description { get; init; }
    public int? DurationMinutes { get; init; }
}

public class UpdateGameValidator : Validator<UpdateGameRequest>
{
    public UpdateGameValidator()
    {
        RuleFor(x => x.GameId).GreaterThan(0);
        Include(new GameValidator());
    }
}

public class UpdateGameEndpoint : Endpoint<UpdateGameRequest, Game>
{
    public override void Configure()
    {
        Patch(string.Empty);
        Group<CollectionGroup>();
    }

    public override Task<Game> ExecuteAsync(UpdateGameRequest req, CancellationToken ct)
    {
        return Task.FromResult<Game>(null);
    }
}
