using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PogGames.Helper;
using PogGames.Model;

namespace PogGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {

        public class NameDTO
        {
            public string gameName { get; set; }
        }
        private readonly PogGamesContext _context;

        public GamesController(PogGamesContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            return await _context.Game.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(string id)
        {
            var game = await _context.Game.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // GET api/Games/SearchByCharacters/HelloWorld
        [HttpGet("SearchByCharacters/{searchString}")]
        public async Task<ActionResult<IEnumerable<Game>>> Search(string searchString)
        {
            var games = await _context.Game.Include(game => game.Character).Select(game => new Game
            {
                GameId = game.GameId,
                GameCompany = game.GameCompany,
                GameName = game.GameName,
                GameRelease = game.GameRelease,
                GameSummary = game.GameSummary,
                Genre = game.Genre,
                CoverImageUrl = game.CoverImageUrl,
                IsFavourite = game.IsFavourite,
                Rating = game.Rating,
                Character = game.Character.Where(chara => chara.CharName.Contains(searchString)).ToList()
            }).ToListAsync();

            //remove all games with empty characters
            games.RemoveAll(game => game.Character.Count == 0);
            return Ok(games);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(string id, Game game)
        {
            if (id != game.GameId)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame([FromBody]NameDTO data)
        {
            Game game;
            String gameName;

            try
            {
                // Constructing the game object from our helper function
                gameName = data.gameName;
                game = GiantBombHelper.GetGameFromName(gameName);
            }
            catch
            {
                return BadRequest("Invalid Game Name");
            }

            //add the game object to database
            _context.Game.Add(game);
            await _context.SaveChangesAsync();

            //Get the ID from the game (same ID from the API)
            string id = game.GameId;

            // This is needed because context are NOT thread safe, therefore we create another context for the following task.
            // We will be using this to insert characters into the database on a separate thread
            // So that it doesn't block the API.
            PogGamesContext tempContext = new PogGamesContext();
            CharactersController characterController = new CharactersController(tempContext);

            // This will be executed in the background.
            Task addCharacters = Task.Run(async () =>
            {
                // Get the list of characters from the GiantBombHelper
                List<Character> characters = new List<Character>();
                characters = GiantBombHelper.GetCharacterFromGameId(id);

                foreach(Character chara in characters)
                {
                    chara.GameId = id;
                    // Add this Character to the database
                    await characterController.PostCharacter(chara);
                }
            });

            //return success code and game object
            return CreatedAtAction("GetGame", new { id = game.GameId }, game);

        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(string id)
        {
            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Game.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        private bool GameExists(string id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }
    }
}
