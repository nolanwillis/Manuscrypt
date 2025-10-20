namespace Manuscrypt.Server.Data.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? PhotoUrl { get; set; }
}
