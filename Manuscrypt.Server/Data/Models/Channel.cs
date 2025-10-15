namespace Manuscrypt.Server.Data.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}

namespace Manuscrypt.Server.Services.Exceptions
{
    public class ChannelDoesNotExistException : Exception
    {
        public ChannelDoesNotExistException(int userId)
            : base($"A channel for user {userId} does not exist.") { }
    }
}
