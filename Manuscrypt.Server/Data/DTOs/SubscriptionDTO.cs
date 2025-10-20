namespace Manuscrypt.Server.Data.DTOs
{
    public class SubscriptionDTO
    {
        public int? Id { get; set; }
        // The user who is subscribing
        public int SubscriberId { get; set; }
        // The user who is being subscribed to
        public int SubscribedToId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
