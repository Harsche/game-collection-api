using System.ComponentModel.DataAnnotations;

namespace GameCollectionAPI.DTOs.Users;

public class UserCreateDto
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public required string Username { get; set; }

    public required string Password { get; set; }
}
