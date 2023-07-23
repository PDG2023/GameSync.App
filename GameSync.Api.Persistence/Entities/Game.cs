namespace GameSync.Api.Persistence.Entities;

public class Game
{
    public int Id { get; set; } 
    public required string Name { get; init; }
}
