namespace Manuscrypt.Server.Data.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        // The user who is subscribing
        public int SubscriberId { get; set; }
        // The user who is being subscribed to
        public int SubscribedToId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation 
        public User Subscriber { get; set; } = null!;
        public User SubscribedTo { get; set; } = null!;
    }
}

namespace Manuscrypt.Server.Services.Exceptions
{
    public class SubscriptionDoesNotExistException : Exception
    {
        public SubscriptionDoesNotExistException(int subscriptionId)
            : base($"A subscription with the id {subscriptionId} does not exist.") { }
    }
}
