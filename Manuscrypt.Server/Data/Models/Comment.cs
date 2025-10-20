namespace Manuscrypt.Server.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}

namespace Manuscrypt.Server.Services.Exceptions
{
    public class CommentDoesNotExistException : Exception
    {
        public CommentDoesNotExistException(int commentId)
            : base($"A comment with the id {commentId} does not exist.") { }
    }
}
