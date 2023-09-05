using GameSync.Api.Persistence.Entities.Games;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSync.Api.Persistence.Entities;

public abstract class PartyGame
{
    public int Id { get; set; }
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

    public int PartyId { get; set; }
    [ForeignKey(nameof(PartyId))]
    public virtual Party Party { get; set;} = null!;

}

[Index(nameof(PartyId), nameof(GameId), IsUnique = true)]
public class PartyCustomGame : PartyGame
{
    public int GameId { get; set; }
    [ForeignKey(nameof(GameId))]
    public virtual CustomGame Game { get; set; } = null!;
}

[Index(nameof(PartyId), nameof(BoardGameGeekId), IsUnique = true)]
public class PartyBoardGameGeekGame : PartyGame
{
    public int BoardGameGeekId { get; set; }

    [ForeignKey(nameof(BoardGameGeekId))]
    public virtual BoardGameGeekGame BoardGameGeekGame { get; set; } = null!;
}
