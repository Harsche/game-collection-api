using GameCollectionAPI.DTOs.Games;

namespace GameCollectionAPI.Models;

public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Genre { get; set; }
    public required string Developer { get; set; }
    public required string Publisher { get; set; }
    public DateTime ReleaseDate { get; set; }
    public required string Platform { get; set; }

    public void UpdateFromDto(GameCreateDto dto)
    {
        Name = dto.Name;
        Genre = dto.Genre;
        Developer = dto.Developer;
        Publisher = dto.Publisher;
        ReleaseDate = dto.ReleaseDate;
        Platform = dto.Platform;
    }

    public static Game FromCreateDto(GameCreateDto dto)
    {
        return new Game
        {
            Name = dto.Name,
            Genre = dto.Genre,
            Developer = dto.Developer,
            Publisher = dto.Publisher,
            ReleaseDate = dto.ReleaseDate,
            Platform = dto.Platform
        };
    }

    public GameReadDto ToReadDto()
    {
        return new GameReadDto
        {
            Id = Id,
            Name = Name,
            Genre = Genre,
            Developer = Developer,
            Publisher = Publisher,
            ReleaseDate = ReleaseDate,
            Platform = Platform
        };
    }
}
