namespace Manuscrypt.Server.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public int Views { get; set; }

        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;          
        public string FileType { get; set; } = string.Empty;          
        public long FileSizeBytes { get; set; }       

        // Navigation
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Tag> PostTags { get; set; } = new List<Tag>();
    }

    public enum Tag
    {
        Fiction,
        Fantasy,
        Syfy,
        Nonfiction,
        NSFW,
        Romance,
        Smut,
        Chapter,
        Page,
        Poem,
        Short,
        Long,
        Scary,
        Dark,
        Raunchy,
        Sensual,
        Funny,
        Heartwarming,
        Lighthearted,
        Political,
        Disturbing,
        Informative
    }
}

namespace Manuscrypt.Server.Services.Exceptions
{
    public class PostDoesNotExistException : Exception
    {
        public PostDoesNotExistException(int postId)
            : base($"A post with the id {postId} does not exist.") { }
    }
}
