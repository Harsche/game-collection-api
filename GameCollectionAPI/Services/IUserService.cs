using GameCollectionAPI.DTOs.Users;

namespace GameCollectionAPI.Services;

public interface IUserService
{
    Task<UserReadDto> CreateUserAsync(UserCreateDto createdUser);
    Task<UserReadDto?> GetUserByIdAsync(int id);
    Task<bool> UpdateUsernameAsync(int id, UpdateUsernameDto updateUsernameDto);
    Task<bool> DeleteUserAsync(int id);
}
