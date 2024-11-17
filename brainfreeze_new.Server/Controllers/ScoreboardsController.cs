﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using brainfreeze_new.Server.Models;

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

        // GET: api/Scoreboards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Scoreboard>> GetScoreboard(int id)
        {
            var scoreboard = await _context.scoreboards.FindAsync(id);

            if (scoreboard == null)
            {
                return NotFound();
            }

            return scoreboard;
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
            _context.scoreboards.Add(scoreboard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScoreboard", new { id = scoreboard.id }, scoreboard);
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
    }
}
