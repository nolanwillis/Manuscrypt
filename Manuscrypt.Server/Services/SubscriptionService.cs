using Manuscrypt.Server.Data;
using Manuscrypt.Server.Data.DTOs.Subscription;
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
        public virtual async Task<IEnumerable<GetSubscriptionDTO>> GetSubscriptionsAsync()
        {
            var subscriptions = await _subscriptionRepo.GetAllAsync();

            var subscriptionDTOs = subscriptions.Select(subscription => new GetSubscriptionDTO
            {
                Id = subscription.Id,
                SubscriberId = subscription.SubscriberId,
                SubscribedToId = subscription.SubscribedToId,
                CreatedAt = subscription.CreatedAt
            }).ToList();

            return subscriptionDTOs;
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
            await _context.SaveChangesAsync();

            return subscription.Id;
        }

        public virtual async Task DeleteSubscriptionAsync(int subscriptionId)
        {
            var subscription = await _subscriptionRepo.GetAsync(subscriptionId);
            if (subscription == null)
            {
                throw new SubscriptionDoesNotExistException(subscriptionId);
            }

            _subscriptionRepo.Delete(subscription);
            _context.SaveChanges();
        }
    }
}
