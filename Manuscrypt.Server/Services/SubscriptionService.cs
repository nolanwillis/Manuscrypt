using Manuscrypt.Server.Data.DTOs.Subscription;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services
{
    public class SubscriptionService
    {
        private readonly SubscriptionRepo _subscriptionRepo;

        public SubscriptionService(SubscriptionRepo subscriptionRepo)
        {
            _subscriptionRepo = subscriptionRepo;
        }

        public virtual async Task<GetSubscriptionDTO> GetSubscriptionAsync(int subscriptionId)
        {
            var subscription = await _subscriptionRepo.GetAsync(subscriptionId);
            if (subscription == null)
            {
                throw new CommentDoesNotExistException(subscriptionId);
            }

            var subscriptionDTO = new GetSubscriptionDTO
            {
                Id = subscription.Id,
                SubscriberId = subscription.SubscriberId,
                SubscribedToId = subscription.SubscribedToId,
                CreatedAt = subscription.CreatedAt
            };

            return subscriptionDTO;
        } 

        public virtual async Task<int> CreateSubscriptionAsync(CreateSubscriptionDTO subscriptionDTO)
        {
            // Add a new Subscription to the DB.
            var subscription = new Subscription
            {
                SubscriberId = subscriptionDTO.SubscriberId,
                SubscribedToId = subscriptionDTO.SubscribedToId,
                CreatedAt = subscriptionDTO.CreatedAt
            };

            await _subscriptionRepo.AddAsync(subscription);

            return subscription.Id;
        }

        public virtual async Task DeleteSubscriptionAsync(int subscriptionId) => await _subscriptionRepo.DeleteAsync(subscriptionId);
    }
}
