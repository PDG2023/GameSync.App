using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Persistence.Entities;

[Index(nameof(BoardGameGeekId), nameof(UserId), IsUnique = true)]
public class BoardGameGeekGame : Game
{

    public required int BoardGameGeekId { get; set; }

}
