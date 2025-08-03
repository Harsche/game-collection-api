using GameCollectionAPI.Data;
using GameCollectionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameCollectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public GamesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves a list of all games from the database.
        /// </summary>
        /// <returns>An asynchronous task that returns an enumerable collection of <see cref="Game"/> objects.</returns>
        [HttpGet]
        public async Task<IEnumerable<Game>> Get() => await _dbContext.Games.ToListAsync();


        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) { return NotFound(); }

            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Game game)
        {
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = game.Id }, game);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Game updatedGame)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) { return NotFound(); }

            game.Name = updatedGame.Name;
            game.Genre = updatedGame.Genre;
            game.Developer = updatedGame.Developer;
            game.Publisher = updatedGame.Publisher;
            game.ReleaseDate = updatedGame.ReleaseDate;
            game.Platform = updatedGame.Platform;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) { return NotFound(); }

            _dbContext.Remove(game);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
