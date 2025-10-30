namespace Manuscrypt.CommentService.Data
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

namespace Manuscrypt.CommentService.Exceptions
{
    public class CommentDoesNotExistException : Exception
    {
        public CommentDoesNotExistException(int commentId)
            : base($"A comment with the id {commentId} does not exist.") {}
    }

    public class UserForCommentDoesNotExistException : Exception
    {
        public UserForCommentDoesNotExistException(int userId)
            : base($"The associated user with the id {userId} for this comment does not exist.") {}
    }
    public class PostForCommentDoesNotExistException : Exception
    {
        public PostForCommentDoesNotExistException(int postId)
            : base($"The associated post with the id {postId} for this comment does not exist.") {}
    }
}
