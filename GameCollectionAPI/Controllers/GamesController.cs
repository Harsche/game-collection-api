using GameCollectionAPI.Data;
using GameCollectionAPI.Models;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IEnumerable<Game>> Get() => await _dbContext.Games.ToListAsync();

        [HttpPost]
        public async Task<IActionResult> Post(Game game)
        {
            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new {id = game.Id}, game);
        } 
    }
}
