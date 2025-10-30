namespace Manuscrypt.SubscriptionService.Data
{
    public class Subscription
    {
        public int Id { get; set; }
        // The user who is subscribing
        public int SubscriberId { get; set; }
        // The user who is being subscribed to
        public int SubscribedToId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

namespace Manuscrypt.SubscriptionService.Exceptions
{
    public class SubscriptionDoesNotExistException : Exception
    {
        public SubscriptionDoesNotExistException(int subscriptionId)
            : base($"A subscription with the id {subscriptionId} does not exist.") {}
    }

    public class SubscriptionAlreadyExistsException : Exception
    {
        public SubscriptionAlreadyExistsException(int subscriberId, int subscribedToId)
            : base($"A subscription with the subscriber id {subscriberId} and subscribedToId {subscribedToId}" +
                  " already exists.") {}
    }

    public class CouldNotFindSubscribeesException : Exception
    {
        public CouldNotFindSubscribeesException(int subscriberId, int subscribedToId)
            : base($"Subscriber with the id {subscriberId} or the subscribed to with the id {subscribedToId} do not exist.") {}
    }
}
