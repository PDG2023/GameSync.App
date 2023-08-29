using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GameSync.Api.Persistence.Entities;

[PrimaryKey(nameof(GameId), nameof(PartyId))]
public class PartyGame
{
    public int GameId { get; set; }
    public int PartyId { get; set; }
    public virtual ICollection<Vote>? Votes { get; set; }
    public virtual Game Game { get; set; } = null!;
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

