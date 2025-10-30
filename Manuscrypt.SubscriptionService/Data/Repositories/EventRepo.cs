using Microsoft.EntityFrameworkCore;
using Manuscrypt.Shared;
using System.Text.Json;

namespace Manuscrypt.SubscriptionService.Data.Repositories;

public class EventRepo
{
    private readonly SubscriptionContext _dbContext;

    public EventRepo(SubscriptionContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Event>> GetAsync(int startId, int endId)
        => await _dbContext.Events.Where(e => e.Id >= startId && e.Id <= endId).ToListAsync();

    public async Task AddCreateSubscriptionEventAsync(object content)
    {
        await _dbContext.Events.AddAsync(new Event
        {
            Type = EventType.CreateSubscription,
            OccuredAt = DateTime.UtcNow,
            ContentJson = JsonSerializer.Serialize(content)
        });
    }
    public async Task AddDeleteSubscriptionEventAsync(object content)
    {
        await _dbContext.Events.AddAsync(new Event
        {
            Type = EventType.DeleteSubscription,
            OccuredAt = DateTime.UtcNow,
            ContentJson = JsonSerializer.Serialize(content)
        });
    }
}
