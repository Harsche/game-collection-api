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

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="createdUser">The data for creating a user.</param>
    /// <returns>A task containing the created user's read DTO.</returns>
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

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A task with a boolean indicating whether the deletion was successful.</returns>
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

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A task containing the user read DTO if found; otherwise, null.</returns>
    public async Task<UserReadDto?> GetUserByIdAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user?.ToReadDto();
    }

    /// <summary>
    /// Updates the username for a specified user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="updateUsernameDto">The username update data.</param>
    /// <returns>A task with a boolean indicating whether the update was successful.</returns>
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
