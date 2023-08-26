﻿using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Persistence.Entities;

[PrimaryKey(nameof(GameId), nameof(PartyId))]
public class PartyGame
{
    public int GameId { get; set; }
    public int PartyId { get; set; }
    public ICollection<Vote>? Votes { get; set; }
}

[Owned]
[Index(nameof(UserId), IsUnique = true)]
public class Vote
{

    /// <summary>
    /// Null = the user has not voted yet. Samey effect if the vote doesn't exist 
    /// </summary>
    public bool? VoteYes { get; set; }

    public required string UserName { get; set; }

    /// <summary>
    /// When null, the vote has been made by a guest
    /// </summary>
    public int? UserId { get; set; }

}

