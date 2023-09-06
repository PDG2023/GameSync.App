namespace GameSync.Api.CommonResponses;

public class GamePreview
{
    public int YearPublished { get; init; }
    public string Name { get; init; }
    public bool IsExpansion { get; init; }
    public int Id { get; init; }
    public string? ThumbnailUrl { get; init; }
    public string? ImageUrl { get; init; }
}
