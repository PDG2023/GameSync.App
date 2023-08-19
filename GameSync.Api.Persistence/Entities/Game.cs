
namespace GameSync.Api.Persistence.Entities;

public class Game
{
    public int Id { get; set; } 
    public required string Name { get; set; }

    public required int MinPlayer { get; set; }
    public required int MaxPlayer { get; set; }
    public required int MinAge { get; set; }

    public int? DurationMinute { get; set; }

    public string? Description { get; set; }

    public required string UserId { get; init; }
}
