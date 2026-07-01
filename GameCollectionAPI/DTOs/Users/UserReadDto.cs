namespace GameCollectionAPI.DTOs.Users;

public class UserReadDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public DateTime CreatedDate { get; set; }
}
