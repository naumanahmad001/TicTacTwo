using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Config> Configurations { get; set; }
    public DbSet<Game> SavedGames { get; set; }
    
    public DbSet<TempGameState> TempStates { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}