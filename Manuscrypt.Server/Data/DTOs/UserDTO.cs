namespace Manuscrypt.Server.Data.DTOs;

public class UserDTO
{
    public int? Id { get; set; }
    public string? DisplayName { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public bool? IsChild { get; set; } = false;
}
