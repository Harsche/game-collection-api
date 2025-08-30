using GameCollectionAPI.DTOs.Users;
using GameCollectionAPI.Exceptions;
using GameCollectionAPI.Models;
using GameCollectionAPI.Repositories;

namespace GameCollectionAPI.Services;

public class UserService : IUserService
{
    private IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<UserReadDto> CreateUserAsync(UserCreateDto createdUser)
    {
        if (await _repo.GetByUsernameAsync(createdUser.Username) != null)
        {
            throw new DuplicateUsernameException();
        }

        var user = User.FromCreateDto(createdUser);
        await _repo.AddAsync(user);
        return user.ToReadDto();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);

        if (user == null)
        {
            return false;
        }

        await _repo.DeleteAsync(user);
        return true;
    }

    public async Task<UserReadDto?> GetUserByIdAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user?.ToReadDto();
    }

    public async Task<bool> UpdateUsernameAsync(int id, UpdateUsernameDto updateUsernameDto)
    {
        if (await _repo.GetByUsernameAsync(updateUsernameDto.NewUsername) != null)
        {
            throw new DuplicateUsernameException();
        }

        var user = await _repo.GetByIdAsync(id);

        if (user == null)
        {
            return false;
        }

        user.Username = updateUsernameDto.NewUsername;
        await _repo.UpdateAsync();
        return true;
    }
}
