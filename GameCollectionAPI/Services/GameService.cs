using GameCollectionAPI.DTOs.Games;
using GameCollectionAPI.Models;
using GameCollectionAPI.Repositories;

namespace GameCollectionAPI.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _repo;

    public GameService(IGameRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Creates a new game in the collection.
    /// </summary>
    /// <param name="createdGame">The game data to create.</param>
    /// <returns>A task containing the created game's read DTO.</returns>
    public async Task<GameReadDto> CreateGameAsync(GameCreateDto createdGame)
    {
        var game = Game.FromCreateDto(createdGame);
        await _repo.AddAsync(game);
        return game.ToReadDto();
    }

    /// <summary>
    /// Deletes a game by ID.
    /// </summary>
    /// <param name="id">The game ID.</param>
    /// <returns>A task with a boolean indicating whether the deletion was successful.</returns>
    public async Task<bool> DeleteGameAsync(int id)
    {
        var game = await _repo.GetByIdAsync(id);

        if (game == null)
        {
            return false;
        }

        await _repo.DeleteAsync(game);
        return true;
    }

    /// <summary>
    /// Retrieves all games from the collection.
    /// </summary>
    /// <returns>A task containing a list of all games read DTOs.</returns>
    public async Task<List<GameReadDto>> GetAllGamesAsync()
    {
        var games = await _repo.GetAllAsync();
        return [.. games.Select(g => g.ToReadDto())];
    }

    /// <summary>
    /// Retrieves a specific game by its ID.
    /// </summary>
    /// <param name="id">The game ID.</param>
    /// <returns>A task containing the game's read DTO if found; otherwise, null.</returns>
    public async Task<GameReadDto?> GetGameByIdAsync(int id)
    {
        var game = await _repo.GetByIdAsync(id);

        if (game == null)
        {
            return null;
        }

        return game.ToReadDto();
    }

    /// <summary>
    /// Updates an existing game with new data.
    /// </summary>
    /// <param name="id">The game ID.</param>
    /// <param name="updatedGame">The updated game data.</param>
    /// <returns>A task with a boolean indicating whether the update was successful.</returns>
    public async Task<bool> UpdateGameAsync(int id, GameCreateDto updatedGame)
    {
        var game = await _repo.GetByIdAsync(id);

        if (game == null)
        {
            return false;
        }

        game.UpdateFromDto(updatedGame);
        await _repo.UpdateAsync();
        return true;
    }
}
