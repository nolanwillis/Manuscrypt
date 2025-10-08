namespace Manuscrypt.Server.Data.Models;

public class Post
{
    public int Id { get; set; }
    public int ChannelId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public int Views { get; set; } = 0;
    public bool IsForChildren { get; set; } = true;

    // Blob-related fields
    public string? FileUrl { get; set; }           // Full URL to the blob (e.g. https://cdn.mysite.com/posts/file123.pdf)
    public string? FileName { get; set; }          // Original filename for display
    public string? FileType { get; set; }          // MIME type (e.g., application/pdf, text/plain)
    public long? FileSizeBytes { get; set; }       // For info or validation
    public DateTime? FileUploadedAt { get; set; }  // Optional tracking

    // Navigation
    public Channel Channel { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Edit> Edits { get; set; } = new List<Edit>();
    public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
}
