namespace GameCollectionAPI.DTOs.Games;

public class GameReadDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Genre { get; set; }
    public required string Developer { get; set; }
    public required string Publisher { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public required string Platform { get; set; }
}
