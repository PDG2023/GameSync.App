namespace GameSync.Api.Endpoints.Users.Me.Games;

public interface GameRequest
{
    public string? Name { get; }
    public int? MinPlayer { get; }
    public int? MaxPlayer { get; }
    public int? MinAge { get; }
    public string? Description { get; }
    public int? DurationMinute { get; }
}
