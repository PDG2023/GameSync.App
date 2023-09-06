namespace GameSync.Api.Persistence.Entities.Games;

public interface IGame
{
    int Id { get;  } 
    string Name { get;  }

    int MinPlayer { get; }
    int MaxPlayer { get;}
    int MinAge { get;  }

    int? DurationMinute { get;  }
    string? ImageUrl { get;  }
    string? ThumbnailUrl { get; }

    string? Description { get;  }
    bool IsExpansion { get;  }
    int YearPublished { get;  }
}
