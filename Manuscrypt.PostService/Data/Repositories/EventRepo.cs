using Microsoft.EntityFrameworkCore;
using Manuscrypt.Shared;
using System.Text.Json;

namespace Manuscrypt.PostService.Data.Repositories;

public class EventRepo
{
    private readonly PostContext _dbContext;

    public EventRepo(PostContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Event>> GetAsync(int startId, int endId)
        => await _dbContext.Events.Where(e => e.Id >= startId && e.Id <= endId).ToListAsync();

    public async Task AddCreatePostEventAsync(object content)
    {
        await _dbContext.Events.AddAsync(new Event
        {
            Type = EventType.CreatePost,
            OccuredAt = DateTime.UtcNow,
            ContentJson = JsonSerializer.Serialize(content)
        });
    }
    public async Task AddDeletePostEventAsync(object content)
    {
        await _dbContext.Events.AddAsync(new Event
        {
            Type = EventType.DeletePost,
            OccuredAt = DateTime.UtcNow,
            ContentJson = JsonSerializer.Serialize(content)
        });
    }
}
