namespace GameSync.Api.Endpoints.Users.Me.Parties;

public class PartyPreview
{
    public required int Id { get; init; }
    public string? Location { get; init; }
    public required string Name { get; init; }
    public required int NumberOfGames { get; init; }
    public DateTime DateTime { get; init; }
}
