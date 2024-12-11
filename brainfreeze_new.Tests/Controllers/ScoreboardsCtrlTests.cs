using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using brainfreeze_new.Server.Controllers;
using brainfreeze_new.Server.Models;
using Microsoft.AspNetCore.Mvc;
using brainfreeze_new.Server.Exceptions;
using Microsoft.AspNetCore.Http;

namespace brainfreeze_new.Server.Controllers.Tests
{
    public class ScoreboardsControllerTests
    {
        private ServiceProvider SetupInMemoryDbContext(out ServiceCollection services)
        {
            services = new ServiceCollection();

            
            services.AddDbContext<ScoreboardDBContext>(options =>
                options.UseInMemoryDatabase("InMemoryTestDb"));

            return services.BuildServiceProvider();
        }

        [Fact]
        public async Task DeleteScoreboard_ValidId_ReturnsNoContent()
        {
            var services = new ServiceCollection();
            var serviceProvider = SetupInMemoryDbContext(out services);
            var context = serviceProvider.GetService<ScoreboardDBContext>();

            
            var scoreboard = new Scoreboard { Id = 1, Username = "User1", CardflipScore = 100, Place = 1, SimonScore = 200, NrgScore = 300 };
            context.Scoreboards.Add(scoreboard);
            await context.SaveChangesAsync();

            var controller = new ScoreboardsController(context);

            
            var result = await controller.DeleteScoreboard(1);

            
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Scoreboards.FindAsync(1));
        }


        [Fact]
        public async Task GetScoreboardByUsername_ExistingUsername_ReturnsScoreboard()
        {
            var services = new ServiceCollection();
            var serviceProvider = SetupInMemoryDbContext(out services);
            var context = serviceProvider.GetService<ScoreboardDBContext>();

            var testUsername = "User2";
            var testScoreboard = new Scoreboard { Id = 2, Username = testUsername, CardflipScore = 500 };
            context.Scoreboards.Add(testScoreboard);
            await context.SaveChangesAsync();

            var controller = new ScoreboardsController(context);
            var result = await controller.GetScoreboardByUsername(testUsername);

            var okResult = Assert.IsType<ActionResult<Scoreboard>>(result);
            var returnedScoreboard = okResult.Value;
            Assert.Equal(testUsername, returnedScoreboard?.Username);
        }
    }
}