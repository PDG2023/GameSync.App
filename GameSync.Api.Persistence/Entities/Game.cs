
namespace GameSync.Api.Persistence.Entities;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }

    public  int MinPlayer { get; set; }
    public int MaxPlayer { get; set; }
    public int MinAge { get; set; }

    public int? DurationMinute { get; set; }
    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public string UserId { get; init; }
}
