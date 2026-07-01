using System.ComponentModel.DataAnnotations;

namespace GameCollectionAPI.DTOs.Games;

public class GameCreateDto
{
    [Required]
    [StringLength(64, MinimumLength = 2)]
    public required string Name { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 2)]
    public required string Genre { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public required string Developer { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 3)]
    public required string Publisher { get; set; }

    [Required]
    public required DateTime ReleaseDate { get; set; }

    [Required]
    [StringLength(32, MinimumLength = 2)]
    public required string Platform { get; set; }
}
