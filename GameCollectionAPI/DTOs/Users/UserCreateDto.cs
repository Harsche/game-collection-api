using System.ComponentModel.DataAnnotations;

namespace GameCollectionAPI.DTOs.Users;

public class UserCreateDto
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public required string Username { get; set; }

    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
}
