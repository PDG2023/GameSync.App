using GameSync.Api.Persistence.Entities.Games;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GameSync.Api.Persistence.Entities;

[PrimaryKey(nameof(GameId), nameof(PartyId))]
public class PartyGame
{
    public int GameId { get; set; }

    public int PartyId { get; set; }
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

    [ForeignKey(nameof(PartyId))]
    public virtual Party Party { get; set;} = null!;

    [ForeignKey(nameof(GameId))]
    public virtual Game Game { get; set; }

}

//public class PartyCustomGame : PartyGame
//{
//    [ForeignKey(nameof(GameId))]
//    public virtual CustomGame Game { get; set; } = null!;
//}

//public class PartyBoardGameGeekGame : PartyGame
//{
//    [ForeignKey(nameof(GameId))]
//    public virtual BoardGameGeekGame BoardGameGeekGame { get; set; } = null!;
//}
