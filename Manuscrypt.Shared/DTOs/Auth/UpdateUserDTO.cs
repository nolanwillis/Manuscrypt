namespace Manuscrypt.Shared.DTOs.User;

public class UpdateUserDTO
{
    public string Id { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? Email { get; set; } 
    public string? PhotoUrl { get; set; }
}
