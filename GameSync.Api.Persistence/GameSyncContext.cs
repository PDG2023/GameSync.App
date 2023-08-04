using GameSync.Api.Identity;
using GameSync.Api.Persistence.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameSync.Api.Persistence;

public class GameSyncContext : IdentityDbContext<User>
{
    public DbSet<Game> Games { get; set; }

    // Needed for migrations
    public GameSyncContext() { }

    public GameSyncContext(DbContextOptions<GameSyncContext> options) : base(options) { }

}