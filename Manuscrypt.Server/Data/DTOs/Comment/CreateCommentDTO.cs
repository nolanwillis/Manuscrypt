namespace Manuscrypt.Server.Data.DTOs.Comment
{
    public class CreateCommentDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
