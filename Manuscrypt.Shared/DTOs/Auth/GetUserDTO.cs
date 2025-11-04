namespace Manuscrypt.Shared.DTOs.User;

public class GetUserDTO
{
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
}
