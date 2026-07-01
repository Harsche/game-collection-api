using GameCollectionAPI.Models;

namespace GameCollectionAPI.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    Task DeleteAsync(User user);
    Task UpdateAsync();
}
