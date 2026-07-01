using GameCollectionAPI.Data;
using GameCollectionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCollectionAPI.Repositories;

public class GameRepository : IGameRepository
{
    private readonly AppDbContext _dbContext;

    public GameRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(Game game)
    {
        _dbContext.Games.Add(game);
        return _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(Game game)
    {
        _dbContext.Remove(game);
        return _dbContext.SaveChangesAsync();
    }

    public Task<List<Game>> GetAllAsync()
    {
        return _dbContext.Games.ToListAsync();
    }

    public Task<Game?> GetByIdAsync(int id)
    {
        return _dbContext.Games.FirstOrDefaultAsync(g => g.Id == id);
    }

    public Task UpdateAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}
