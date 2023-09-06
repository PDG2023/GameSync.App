
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSync.Api.Persistence.Entities;

public class Party
{
    public int Id { get; set; }
    [ProtectedPersonalData]
    public string? Location { get; set; }

    [ProtectedPersonalData]
    public string Name { get; set; }
    [ProtectedPersonalData]
    public DateTime DateTime { get; set; }
    public string UserId { get; set; }

    public virtual ICollection<PartyGame>? Games { get; set; }

    public string? InvitationToken { get; set; }

}