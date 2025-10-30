namespace Manuscrypt.Shared.DTOs.Post;

public class GetPostDTO
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public int Views {  get; set; }
    public string FileUrl { get; set; } = string.Empty;
}
