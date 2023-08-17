using GameSync.Api.Persistence.Entities;

namespace GameSync.Api.Endpoints.Users.Me.Collection;


public class CreateGameRequest
{
    public required string Name { get; init; }
    public required int MinPlayer { get; init; }
    public required int MaxPlayer { get; init; }
    public required int MinAge { get; init; }
}

public class CreateGameResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required int MinPlayer { get; init; }
    public required int MaxPlayer { get; init; }
    public required int MinAge { get; init; }
}

public class CreateGameEndpoint : Endpoint<CreateGameRequest, CreateGameResponse>
{
    public override void Configure()
    {
        Post(string.Empty);
        Group<CollectionGroup>();
    }
    public override Task<CreateGameResponse> ExecuteAsync(CreateGameRequest req, CancellationToken ct)
    {
        return Task.FromResult(new CreateGameResponse
        {
            Id = string.Empty,
            Name = string.Empty,
            MinPlayer = 0,
            MaxPlayer = 0,
            MinAge = 0
        });
    }
}
