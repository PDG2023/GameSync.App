using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSync.Api.Persistence.Entities.Games;


public class UserBoardGameGeekGame : Game
{
    public int BoardGameGeekGameId { get; set; }
    [ForeignKey(nameof(BoardGameGeekGameId))]
    public virtual BoardGameGeekGame BoardGameGeekGame { get; set;} 

}
