using Manuscrypt.SubscriptionService.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.SubscriptionService.Data.Repositories;

public class SubscriptionRepo 
{
    private readonly SubscriptionContext _dbContext;

    public SubscriptionRepo(SubscriptionContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<Subscription?> GetAsync(int subscriptionId) 
        => await _dbContext.Subscriptions.FindAsync(subscriptionId);

    public virtual async Task<Subscription?> GetAsync(int subscriberId, int subscribedToId)
    {
        var subscription = await _dbContext.Subscriptions
            .Where(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId)
            .FirstOrDefaultAsync();

        return subscription;
    }  

    public virtual async Task<IEnumerable<Subscription>> GetSubscriptionsForUserAsync(int userId)
        => await _dbContext.Subscriptions.Where(s => s.SubscriberId == userId).ToListAsync();

    public virtual async Task<IEnumerable<Subscription>> GetSubscribersForUserAsync(int userId)
        => await _dbContext.Subscriptions.Where(s => s.SubscribedToId == userId).ToListAsync();

    public virtual async Task CreateAsync(Subscription subscription)
    {
        await _dbContext.AddAsync(subscription);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int subscriptionId)
    {
        var entity = await _dbContext.Subscriptions.FindAsync(subscriptionId);
        if (entity == null)
        {
            throw new SubscriptionDoesNotExistException(subscriptionId);
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
