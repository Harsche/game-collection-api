using GameCollectionAPI.DTOs;
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

    public async Task<GameReadDto> CreateGameAsync(GameCreateDto createdGame)
    {
        var game = Game.FromCreateDto(createdGame);
        await _repo.AddAsync(game);
        return game.ToReadDto();
    }

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

    public async Task<List<GameReadDto>> GetAllGamesAsync()
    {
        var games = await _repo.GetAllAsync();
        return [.. games.Select(g => g.ToReadDto())];
    }

    public async Task<GameReadDto?> GetGameByIdAsync(int id)
    {
        var game = await _repo.GetByIdAsync(id);

        if (game == null)
        {
            return null;
        }

        return game.ToReadDto();
    }

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
