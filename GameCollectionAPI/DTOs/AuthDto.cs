using System.ComponentModel.DataAnnotations;

namespace GameCollectionAPI.DTOs;

public class AuthDto
{
    [Required]
    [MinLength(3)]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}
