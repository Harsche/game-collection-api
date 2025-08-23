using GameCollectionAPI.DTOs;

namespace GameCollectionAPI.Services;

public interface IGameService
{
    Task<List<GameReadDto>> GetAllGamesAsync();
    Task<GameReadDto?> GetGameByIdAsync(int id);
    Task<GameReadDto> CreateGameAsync(GameCreateDto createdGame);
    Task<bool> DeleteGameAsync(int id);
    Task<bool> UpdateGameAsync(int id, GameCreateDto updatedGame);
}
