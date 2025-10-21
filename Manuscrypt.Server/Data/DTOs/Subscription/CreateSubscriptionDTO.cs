namespace Manuscrypt.Server.Data.DTOs.Subscription
{
    public class CreateSubscriptionDTO
    {
        public int SubscriberId { get; set; }
        // The user who is being subscribed to
        public int SubscribedToId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
