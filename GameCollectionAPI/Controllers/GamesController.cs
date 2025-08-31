using GameCollectionAPI.DTOs.Games;
using GameCollectionAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCollectionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _service;

        public GamesController(IGameService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets all games in the collection.
        /// </summary>
        /// <returns>List of games.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<GameReadDto>> Get() => await _service.GetAllGamesAsync();

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

            var game = await _service.GetGameByIdAsync(id);
            return game == null ? NotFound() : Ok(game);
        }

        /// <summary>
        /// Adds a new game to the collection.
        /// </summary>
        /// <param name="createdGame">Game object to add.</param>
        /// <returns>The created game.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] GameCreateDto createdGame)
        {
            if (createdGame == null) { return BadRequest(); }

            var game = await _service.CreateGameAsync(createdGame);

            return CreatedAtAction(nameof(Get), new { id = game.Id }, game);
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

            bool successful = await _service.UpdateGameAsync(id, updatedGame);
            return successful ?  NoContent() : NotFound();
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

            bool successful = await _service.DeleteGameAsync(id);
            return successful ?  NoContent() : NotFound();
        }
    }
}
