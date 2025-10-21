namespace Manuscrypt.Server.Data.DTOs.User;

public class GetUserDTO
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
}
