using GameCollectionAPI.Models;

namespace GameCollectionAPI.Repositories;

public interface IGameRepository
{
    Task<List<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(int id);
    Task AddAsync(Game game);
    Task UpdateAsync();
    Task DeleteAsync(Game game);
}
