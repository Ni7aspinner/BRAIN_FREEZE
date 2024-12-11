using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using brainfreeze_new.Server.Models;
using Xunit;

namespace brainfreeze_new.Tests.Models
{
    public class ScoreboardDBContextTests
    {
        private DbContextOptions<ScoreboardDBContext> CreateNewContextOptions()
        {
            // Use InMemory Database for testing
            return new DbContextOptionsBuilder<ScoreboardDBContext>()
                .UseInMemoryDatabase(databaseName: "ScoreboardTestDB")
                .Options;
        }

        [Fact]
        public async Task Can_Add_Scoreboard_Entry()
        {
            var options = CreateNewContextOptions();
            using (var context = new ScoreboardDBContext(options))
            {
                // Arrange: Create a new Scoreboard entity
                var scoreboardEntry = new Scoreboard
                {
                    Place = 1,
                    Username = "Player1",
                    SimonScore = 150,
                    CardflipScore = 200,
                    NrgScore = 100
                };

                // Act: Add the entity to the DbContext and save changes
                context.Scoreboards.Add(scoreboardEntry);
                await context.SaveChangesAsync();

                // Assert: Check if the entity was added correctly
                var savedScoreboard = await context.Scoreboards
                    .Where(s => s.Username == "Player1")
                    .FirstOrDefaultAsync();

                Assert.NotNull(savedScoreboard);
                Assert.Equal("Player1", savedScoreboard.Username);
                Assert.Equal(1, savedScoreboard.Place);
                Assert.Equal(150, savedScoreboard.SimonScore);
                Assert.Equal(200, savedScoreboard.CardflipScore);
                Assert.Equal(100, savedScoreboard.NrgScore);
            }
        }
    }
}
