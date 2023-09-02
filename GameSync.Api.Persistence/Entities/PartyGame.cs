using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GameSync.Api.Persistence.Entities;

[PrimaryKey(nameof(GameId), nameof(PartyId))]
public class PartyGame
{
    [ForeignKey(nameof(GameId))]
    public int GameId { get; set; }

    [ForeignKey(nameof(PartyId))]
    public int PartyId { get; set; }
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    public virtual Game Game { get; set; } = null!;
    public virtual Party Party { get; set;} = null!;
}

[Owned]
[Index(nameof(UserId), IsUnique = true)]
public class Vote
{
    /// <summary>
    /// Null = the user has not voted yet. Samey effect if the vote doesn't exist 
    /// </summary>
    public bool? VoteYes { get; set; }

    public string? UserName { get; set; }

    /// <summary>
    /// When null, the vote has been made by a guest
    /// </summary>
    [ForeignKey(nameof(User))]
    public string? UserId { get; set; }
    public virtual User? User { get; set; }

}

