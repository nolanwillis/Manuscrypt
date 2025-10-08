namespace Manuscrypt.Server.Data.Models;

public class Channel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
