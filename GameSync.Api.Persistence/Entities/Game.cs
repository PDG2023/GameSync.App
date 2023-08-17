using System.ComponentModel.DataAnnotations;

namespace GameSync.Api.Persistence.Entities;

public class Game
{
    [Key]
    public string? Id { get; set; } 
    public required string Name { get; init; }

    public required int MinPlayer { get; init; }
    public required int MaxPlayer { get; init; }
    public required int MinAge { get; init; }

    public required string UserId { get; init; }
    public User? User { get; set; }
}
