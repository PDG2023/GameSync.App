using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Persistence.Entities;

[Index(nameof(BoardGameGeekId), IsUnique = true)]
public class BoardGameGeekGame : Game
{

    public required int BoardGameGeekId { get; set; }

}
