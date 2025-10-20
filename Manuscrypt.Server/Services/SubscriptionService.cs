using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Data.Repositories;
using Manuscrypt.Server.Services.Exceptions;

namespace Manuscrypt.Server.Services
{
    public class SubscriptionService
    {
        private readonly ManuscryptContext _context;
        private readonly SubscriptionRepo _subscriptionRepo;

        public SubscriptionService(ManuscryptContext context, SubscriptionRepo subscriptionRepo)
        {
            _context = context;
            _subscriptionRepo = subscriptionRepo;
        }

        public async Task<SubscriptionDTO> GetSubscriptionAsync(int subscriptionId)
        {
            var subscription = await _subscriptionRepo.GetAsync(subscriptionId);
            if (subscription == null)
            {
                throw new CommentDoesNotExistException(subscriptionId);
            }

            var subscriptionDTO = new SubscriptionDTO
            {
                Id = subscription.Id,
                SubscriberId = subscription.SubscriberId,
                SubscribedToId = subscription.SubscribedToId,
                CreatedAt = subscription.CreatedAt
            };

            return subscriptionDTO;
        }
        public async Task<IEnumerable<SubscriptionDTO>> GetSubscriptionsAsync()
        {
            var subscriptions = await _subscriptionRepo.GetAllAsync();

            var subscriptionDTOs = subscriptions.Select(subscription => new SubscriptionDTO
            {
                Id = subscription.Id,
                SubscriberId = subscription.SubscriberId,
                SubscribedToId = subscription.SubscribedToId,
                CreatedAt = subscription.CreatedAt
            }).ToList();

            return subscriptionDTOs;
        }

        public async Task<int> CreateSubscriptionAsync(SubscriptionDTO subscriptionDTO)
        {
            // Add a new Subscription to the DB.
            var subscription = new Subscription
            {
                SubscriberId = subscriptionDTO.SubscriberId,
                SubscribedToId = subscriptionDTO.SubscribedToId,
                CreatedAt = subscriptionDTO.CreatedAt
            };

            await _subscriptionRepo.AddAsync(subscription);
            await _context.SaveChangesAsync();

            return subscription.Id;
        }
    }
}
