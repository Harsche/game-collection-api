using GameCollectionAPI.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace GameCollectionAPI.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime CreatedDate { get; set; }

    public static User FromCreateDto(UserCreateDto createDto)
    {
        var user = new User
        {
            Username = createDto.Username,
            CreatedDate = DateTime.UtcNow
        };

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, createDto.Password);

        return user;
    }

    public UserReadDto ToReadDto()
    {
        return new UserReadDto
        {
            Id = Id,
            Username = Username,
            CreatedDate = CreatedDate
        };
    }
}
