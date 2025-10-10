namespace Manuscrypt.Server.Data.DTOs;

public class PostDTO
{
    public int? Id { get; set; }
    public int ChannelId { get; set; } 
    public string? ChannelName { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public int? Views { get; set; } = 0;
    public bool IsForChildren { get; set; } = true;

    // File information fields
    public string? FileUrl { get; set; }
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public long? FileSizeBytes { get; set; } 
}
