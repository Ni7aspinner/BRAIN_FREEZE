using Microsoft.EntityFrameworkCore;

namespace brainfreeze_new.Server.Models
{
    public class ScoreboardDBContext : DbContext
    {
        public ScoreboardDBContext(DbContextOptions<ScoreboardDBContext> options):base(options)
        {

        }

        public DbSet<Scoreboard> scoreboards { get; set; }
    }
}
