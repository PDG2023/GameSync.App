namespace GameSync.Api.CommonResponses;

public class GameDetail : GamePreview
{
    public int MinPlayer { get; set; }
    public int MaxPlayer { get; set; }
    public int MinAge { get; set; }

    public int? DurationMinute { get; set; }

    public string? Description { get; set; }
}
