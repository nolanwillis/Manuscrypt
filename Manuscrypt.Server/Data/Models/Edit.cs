namespace Manuscrypt.Server.Data.Models;

public class Edit
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int SubscriberId { get; set; }
    public string? OriginalText { get; set; }
    public string SuggestedText { get; set; } = string.Empty;
    public EditStatus Status { get; set; } = EditStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Post Post { get; set; } = null!;
    public User User { get; set; } = null!;
}

public enum EditStatus
{
    Pending,
    Accepted,
    Rejected
}
