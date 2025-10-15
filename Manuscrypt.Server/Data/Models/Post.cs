namespace Manuscrypt.Server.Data.Models;

public class Post
{
    public int Id { get; set; }
    public int ChannelId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public int Views { get; set; }

    public string FileUrl { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;          
    public string FileType { get; set; } = string.Empty;          
    public long FileSizeBytes { get; set; }       

    // Navigation
    public Channel Channel { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Edit> Edits { get; set; } = new List<Edit>();
    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
}
