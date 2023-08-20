namespace GameSync.Business.BoardGameGeek.Models;

public class BoardGameGeekGame : IGame
{
    public string? Name { get; init; }

    public int? MinPlayer { get; init; }

    public int? MaxPlayer { get; init; }

    public int? MinAge { get; init; }

    public string? Description { get; init; }

    public int? DurationMinute { get; init; }
}
