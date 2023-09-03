using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Persistence.Entities;

public class User : IdentityUser
{
    public virtual ICollection<Game>? Games { get; set;}
    public virtual ICollection<Party>? Parties { get; set; }
}
