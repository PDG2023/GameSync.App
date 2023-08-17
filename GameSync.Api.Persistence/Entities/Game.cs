using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSync.Api.Persistence.Entities;

public class Game
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public string? Id { get; set; } 
    public required string Name { get; set; }

    public required int MinPlayer { get; set; }
    public required int MaxPlayer { get; set; }
    public required int MinAge { get; set; }

    public int? Duration { get; set; }

    public string? Description { get; set; }

    public required string UserId { get; init; }
    public User? User { get; set; }
}
