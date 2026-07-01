using GameCollectionAPI.Data;
using GameCollectionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCollectionAPI.Repositories;

public class UserRepository : IUserRepository
{
    private AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(User user)
    {
        _context.Users.Add(user);
        return _context.SaveChangesAsync();
    }

    public Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        return _context.SaveChangesAsync();
    }

    public Task<User?> GetByIdAsync(int id)
    {
        return _context.Users.Include(u => u.Role).FirstOrDefaultAsync(user => user.Id == id);
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return _context.Users.Include(u => u.Role).FirstOrDefaultAsync(user => user.Username == username);
    }

    public Task UpdateAsync()
    {
        return _context.SaveChangesAsync();
    }
}
