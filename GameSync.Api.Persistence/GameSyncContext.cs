using GameSync.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Persistence;

public class GameSyncContext : DbContext
{
    public DbSet<Game> Games { get; set; }

    // Needed for migrations
    public GameSyncContext() { }

    public GameSyncContext(DbContextOptions<GameSyncContext> options) : base(options) { }
}