using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Persistence.Entities;

public class User : IdentityUser
{
    public virtual IEnumerable<Game>? Games { get; set;}
    public virtual IEnumerable<Party>? Parties { get; set; }
}
