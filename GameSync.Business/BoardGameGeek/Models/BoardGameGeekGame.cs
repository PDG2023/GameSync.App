namespace GameSync.Business.BoardGameGeek.Models;

public class BoardGameGeekGame
{
    public required int Id { get; init; }
    public required string? Name { get; init; }

    public int? MinPlayer { get; init; }

    public int? MaxPlayer { get; init; }

    public int? MinAge { get; init; }

    public string? Description { get; init; }

    public int? DurationMinute { get; init; }
}
