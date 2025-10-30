using Microsoft.EntityFrameworkCore;
using Manuscrypt.Shared;
using System.Text.Json;

namespace Manuscrypt.CommentService.Data.Repositories;

public class EventRepo
{
    private readonly CommentContext _dbContext;

    public EventRepo(CommentContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Event>> GetAsync(int startId, int endId)
        => await _dbContext.Events.Where(e => e.Id >= startId && e.Id <= endId).ToListAsync();

    public async Task AddCreateCommentEventAsync(object content)
    {
        await _dbContext.Events.AddAsync(new Event
        {
            Type = EventType.CreateComment,
            OccuredAt = DateTime.UtcNow,
            ContentJson = JsonSerializer.Serialize(content)
        });
    }
    public async Task AddDeleteCommentEventAsync(object content)
    {
        await _dbContext.Events.AddAsync(new Event
        {
            Type = EventType.DeleteComment,
            OccuredAt = DateTime.UtcNow,
            ContentJson = JsonSerializer.Serialize(content)
        });
    }
}
