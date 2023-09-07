using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSync.Api.Persistence.Entities;

[Owned]
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

