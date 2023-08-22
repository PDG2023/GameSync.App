﻿
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSync.Api.Persistence.Entities;

public class Party
{
    public int Id { get; set; }
    public required string Location { get; set; }
    public required string Name { get; set; }
    public required DateTime DateTimeOfParty { get; set; }
    public required string UserId { get; set; }

    [Column(TypeName = "jsonb")]
    public IEnumerable<PartyGame>? Games { get; set; }
}