namespace Manuscrypt.Server.Data.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}

namespace Manuscrypt.Server.Services.Exceptions
{
    public class ChannelNameTakenException : Exception
    {
        public ChannelNameTakenException(string name)
            : base($"A channel with the name {name} already exists.") { }
    }
}
