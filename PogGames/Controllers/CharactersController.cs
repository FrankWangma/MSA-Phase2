using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PogGames.Model;

namespace PogGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly PogGamesContext _context;

        public CharactersController(PogGamesContext context)
        {
            _context = context;
        }

        // GET: api/Characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacter()
        {
            return await _context.Character.ToListAsync();
        }

        // GET: api/Characters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(string id)
        {
            var character = await _context.Character.FindAsync(id);

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }

        // PUT: api/Characters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter(string id, Character character)
        {
            if (id != character.ApiCharId)
            {
                return BadRequest();
            }

            _context.Entry(character).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacterExists(id))
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

        // POST: api/Characters
        [HttpPost]
        public async Task<ActionResult<Character>> PostCharacter(Character character)
        {
            _context.Character.Add(character);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CharacterExists(character.ApiCharId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCharacter", new { id = character.ApiCharId }, character);
        }

        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Character>> DeleteCharacter(string id)
        {
            var character = await _context.Character.FindAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            _context.Character.Remove(character);
            await _context.SaveChangesAsync();

            return character;
        }

        private bool CharacterExists(string id)
        {
            return _context.Character.Any(e => e.ApiCharId == id);
        }
    }
}
