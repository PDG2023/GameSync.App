using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSync.Api.Persistence.Entities.Games;


public abstract class UserGame
{
    public int Id { get; set; }
    public string UserId { get; init; }
    [ForeignKey(nameof(UserId))]    
    public virtual User User { get; set; }

}
