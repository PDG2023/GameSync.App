using GameSync.Api.Persistence.Entities;
using GameSync.Api.Persistence.Entities.Games;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace GameSync.Api.Persistence;

public class GameSyncContext : IdentityDbContext<User>
{
    public DbSet<CustomGame> CustomGames { get; set; }
    public DbSet<BoardGameGeekGame> BoardGameGeekGames { get; set; }
    public DbSet<UserBoardGameGeekGame> UserBoardGameGeekGames { get; set; }
    public DbSet<Party> Parties { get; set; }
    public DbSet<PartyGame> PartiesGames { get; set; }
    public DbSet<Game> Games { get; set; }  


    // Needed for migrations
    public GameSyncContext() { }

    public GameSyncContext(DbContextOptions<GameSyncContext> options) : base(options) {
        
    }
}