namespace Manuscrypt.Server.Data.DTOs;

public class PostDTO
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public int Views {  get; set; }
    public string FileUrl { get; set; } = string.Empty;
}
