using GameSync.Api.Persistence.Entities.Games;

namespace GameSync.Api.CommonResponses;

public class GameDetail : GamePreview, IGame
{
    public GameDetail()
    {
        
    }

    public GameDetail(IGame game)
    {
        Id = game.Id;
        ImageUrl = game.ImageUrl;
        Name = game.Name;
        ThumbnailUrl = game.ThumbnailUrl;
        YearPublished = game.YearPublished;
        Description = game.Description;
        DurationMinute = game.DurationMinute;
        IsExpansion = game.IsExpansion;
        MaxPlayer = game.MaxPlayer;
        MinAge = game.MinAge;
        MinPlayer = game.MinPlayer;
    }

    public int MinPlayer { get; set; }
    public int MaxPlayer { get; set; }
    public int MinAge { get; set; }

    public int? DurationMinute { get; set; }

    public string? Description { get; set; }

}
