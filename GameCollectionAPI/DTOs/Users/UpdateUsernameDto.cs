using System.ComponentModel.DataAnnotations;

namespace GameCollectionAPI.DTOs.Users;

public class UpdateUsernameDto
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public required string NewUsername { get; set; }
}
