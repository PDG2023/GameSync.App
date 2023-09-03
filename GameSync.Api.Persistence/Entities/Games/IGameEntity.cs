namespace GameSync.Api.Persistence.Entities.Games;

internal interface IGameEntity
{
    public string Name { get; set; }

    public int MinPlayer { get; set; }
    public int MaxPlayer { get; set; }
    public int MinAge { get; set; }

    public int? DurationMinute { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }

    public string? Description { get; set; }
    public bool IsExpansion { get; set; }
    public int YearPublished { get; set; }
}
