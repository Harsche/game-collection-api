using GameCollectionAPI.DTOs;

namespace GameCollectionAPI.Models;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public string Developer { get; set; }
    public string Publisher { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Platform { get; set; }

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
