using GameCollectionAPI.Data;
using GameCollectionAPI.DTOs;
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
        /// Gets all games in the collection.
        /// </summary>
        /// <returns>List of games.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<GameReadDto>> Get() => await _dbContext.Games
            .Select(game => game.ToReadDto())
            .ToListAsync();

        /// <summary>
        /// Gets a specific game by its ID.
        /// </summary>
        /// <param name="id">Game ID.</param>
        /// <returns>The requested game.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) { return BadRequest(); }

            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) { return NotFound(); }

            return Ok(game.ToReadDto());
        }

        /// <summary>
        /// Adds a new game to the collection.
        /// </summary>
        /// <param name="createdGame">Game object to add.</param>
        /// <returns>The created game.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] GameCreateDto createdGame)
        {
            if (createdGame == null) { return BadRequest(); }

            var game = Game.FromCreateDto(createdGame);
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = game.Id }, game.ToReadDto());
        }

        /// <summary>
        /// Updates an existing game.
        /// </summary>
        /// <param name="id">Game ID.</param>
        /// <param name="updatedGame">Updated game object.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] GameCreateDto updatedGame)
        {
            if (id <= 0 || updatedGame == null) { return BadRequest(); }

            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) { return NotFound(); }

            game.UpdateFromDto(updatedGame);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes a game by its ID.
        /// </summary>
        /// <param name="id">Game ID.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) { return BadRequest(); }

            var game = await _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) { return NotFound(); }

            _dbContext.Remove(game);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
