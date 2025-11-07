using Manuscrypt.Shared;
using Manuscrypt.Shared.DTOs.Subscription;
using Manuscrypt.SubscriptionService.Data;
using Manuscrypt.SubscriptionService.Data.Repositories;
using Manuscrypt.SubscriptionService.Exceptions;

namespace Manuscrypt.SubscriptionService;

public class SubscriptionDomainService
{
    private readonly SubscriptionRepo _subscriptionRepo;
    private readonly EventRepo _eventRepo;
    private readonly AuthServiceClient _authServiceClient;

    public SubscriptionDomainService(SubscriptionRepo subscriptionRepo, EventRepo eventRepo, 
        AuthServiceClient authServiceClient)
    {
        _subscriptionRepo = subscriptionRepo;
        _eventRepo = eventRepo; 
        _authServiceClient = authServiceClient;
    }

    public virtual async Task<GetSubscriptionDTO> GetSubscriptionAsync(int subscriptionId)
    {
        var subscription = await _subscriptionRepo.GetAsync(subscriptionId);
        if (subscription == null)
        {
            throw new SubscriptionDoesNotExistException(subscriptionId);
        }

        var getSubscriptionDTO = new GetSubscriptionDTO
        {
            Id = subscription.Id,
            SubscriberId = subscription.SubscriberId,
            SubscribedToId = subscription.SubscribedToId,
            CreatedAt = subscription.CreatedAt
        };

        return getSubscriptionDTO;
    }

    public virtual async Task<IEnumerable<GetSubscriptionDTO>> GetSubscriptionsForUserAsync(int userId)
    {
        var subscriptions = await _subscriptionRepo.GetSubscriptionsForUserAsync(userId);

        var getSubscriptionDTOs = subscriptions.Select(subscription => new GetSubscriptionDTO
        {
            Id = subscription.Id,
            SubscriberId = subscription.SubscriberId,
            SubscribedToId = subscription.SubscribedToId,
            CreatedAt = subscription.CreatedAt
        });

        return getSubscriptionDTOs;
    }

    public virtual async Task<IEnumerable<GetSubscriptionDTO>> GetSubscribersForUserAsync(int userId)
    {
        var subscriptions = await _subscriptionRepo.GetSubscribersForUserAsync(userId);

        var getSubscriptionDTOs = subscriptions.Select(subscription => new GetSubscriptionDTO
        {
            Id = subscription.Id,
            SubscriberId = subscription.SubscriberId,
            SubscribedToId = subscription.SubscribedToId,
            CreatedAt = subscription.CreatedAt
        });

        return getSubscriptionDTOs;
    }

    public virtual async Task<IEnumerable<Event>> GetEventsAsync(int startId, int endId)
        => await _eventRepo.GetAsync(startId, endId);
    
    public virtual async Task<GetSubscriptionDTO> CreateSubscriptionAsync(CreateSubscriptionDTO subscriptionDTO)
    {
        // Verify the subscription is not a duplicate.
        Subscription? existingSubscription = 
            await _subscriptionRepo.GetAsync(subscriptionDTO.SubscriberId, subscriptionDTO.SubscribedToId);
        if (existingSubscription != null)
        {
            throw new SubscriptionAlreadyExistsException(subscriptionDTO.SubscriberId, subscriptionDTO.SubscribedToId);
        }

        // Verify the users involved in the subscription exist.
        try
        {
            var subscriber = await _authServiceClient.GetAsync(subscriptionDTO.SubscriberId);
            var subscribedTo = await _authServiceClient.GetAsync(subscriptionDTO.SubscribedToId);
        }
        catch (HttpRequestException)
        {
            throw new CouldNotFindSubscribeesException(subscriptionDTO.SubscriberId, subscriptionDTO.SubscribedToId);
        }

        var subscription = new Subscription
        {
            SubscriberId = subscriptionDTO.SubscriberId,
            SubscribedToId = subscriptionDTO.SubscribedToId,
            CreatedAt = subscriptionDTO.CreatedAt
        };

        await _subscriptionRepo.CreateAsync(subscription);
        await _eventRepo.AddCreateSubscriptionEventAsync(subscription);

        var getSubscriptionDTO = new GetSubscriptionDTO
        {
            Id = subscription.Id,
            SubscriberId = subscription.SubscriberId,
            SubscribedToId = subscription.SubscribedToId,
            CreatedAt = subscriptionDTO.CreatedAt
        };

        return getSubscriptionDTO;
    }

    public virtual async Task DeleteSubscriptionAsync(int subscriptionId)
    {
        await _subscriptionRepo.DeleteAsync(subscriptionId);
        await _eventRepo.AddDeleteSubscriptionEventAsync(new { Id = subscriptionId });
    }
}
