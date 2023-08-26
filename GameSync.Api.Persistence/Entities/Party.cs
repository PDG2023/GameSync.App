
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSync.Api.Persistence.Entities;

public class Party
{
    public int Id { get; set; }
    public string? Location { get; set; }
    public required string Name { get; set; }
    public required DateTime DateTime { get; set; }
    public required string UserId { get; set; }

    public virtual ICollection<PartyGame>? Games { get; set; }
}