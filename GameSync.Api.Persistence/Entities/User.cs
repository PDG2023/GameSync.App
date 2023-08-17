using Microsoft.AspNetCore.Identity;

namespace GameSync.Api.Persistence.Entities;

public class User : IdentityUser
{
    public IEnumerable<Game>? PersonalGames { get; set;}
}
