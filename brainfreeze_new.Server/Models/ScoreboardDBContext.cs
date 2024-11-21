using Microsoft.EntityFrameworkCore;

namespace brainfreeze_new.Server.Models
{
    public class ScoreboardDBContext(DbContextOptions<ScoreboardDBContext> options) : DbContext(options)
    {
        public DbSet<Scoreboard> Scoreboards { get; set; }
    }
}
