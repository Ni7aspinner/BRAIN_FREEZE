using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using brainfreeze_new.Server.Models;
using brainfreeze_new.Server.Exceptions;

namespace brainfreeze_new.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreboardsController : ControllerBase
    {
        private readonly ScoreboardDBContext _context;

        public ScoreboardsController(ScoreboardDBContext context)
        {
            _context = context;
        }

        // GET: api/Scoreboards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scoreboard>>> Getscoreboards()
        {
            return await _context.scoreboards.ToListAsync();
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<Scoreboard>> GetScoreboardById(int id)
        {
            try
            {
                var scoreboard = await _context.scoreboards.FindAsync(id);

                if (scoreboard == null)
                {
                    throw new ResourceNotFoundException($"Scoreboard with ID {id} not found.");
                }

                return scoreboard;
            }
            catch (ResourceNotFoundException ex)
            {
                LogException(ex);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                LogException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }


        // GET: api/Scoreboards/get-by-username/
        [HttpGet("get-by-username/{username}")]
        public async Task<ActionResult<Scoreboard>> GetScoreboardByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            try
            {
                var scoreboard = await _context.scoreboards
                    .FirstOrDefaultAsync(s => s.username == username);

                if (scoreboard == null)
                {

                    throw new ResourceNotFoundException($"Scoreboard with username '{username}' not found.");
                }

                return scoreboard;
            }
            catch (ResourceNotFoundException ex)
            {
                LogException(ex);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                LogException(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }




        // PUT: api/Scoreboards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScoreboard(int id, Scoreboard scoreboard)
        {
            scoreboard.id = id;

            _context.Entry(scoreboard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoreboardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Scoreboards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Scoreboard>> PostScoreboard(Scoreboard scoreboard)
        {
            try
            {
                _context.scoreboards.Add(scoreboard);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetScoreboardById), new { id = scoreboard.id }, scoreboard);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating user: " + ex.Message);
            }
        }


        // DELETE: api/Scoreboards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScoreboard(int id)
        {
            var scoreboard = await _context.scoreboards.FindAsync(id);
            if (scoreboard == null)
            {
                return NotFound();
            }

            _context.scoreboards.Remove(scoreboard);
            await _context.SaveChangesAsync();

            return NoContent();
        }   

        private bool ScoreboardExists(int id)
        {
            return _context.scoreboards.Any(e => e.id == id);
        }

        private void LogException(Exception ex)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(basePath, "logs");
            string logPath = Path.Combine(directoryPath, "logs.log");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            System.IO.File.AppendAllText(logPath, $"{DateTime.Now}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}");
        }

    }
}
