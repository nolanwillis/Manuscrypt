namespace Manuscrypt.PostService.Data
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public int Views { get; set; }
        public ICollection<string> Tags { get; set; } = new List<string>();

        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
    }
}

namespace Manuscrypt.PostService.Exceptions
{
    public class PostDoesNotExistException : Exception
    {
        public PostDoesNotExistException(int postId)
            : base($"A post with the id {postId} does not exist.") {}
    }
    public class UserForPostDoesNotExistException : Exception
    {
        public UserForPostDoesNotExistException(int userId)
            : base($"The associated user id {userId} for the post does not exist.") {}
    }
}
