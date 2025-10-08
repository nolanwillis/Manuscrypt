namespace Manuscrypt.Server.Data.Models;

public class Subscription
{
    public int Id { get; set; }
    public int ChannelId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Channel Channel { get; set; } = null!;
    public User User { get; set; } = null!;
}
