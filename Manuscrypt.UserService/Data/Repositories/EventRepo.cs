using Microsoft.EntityFrameworkCore;
using Manuscrypt.Shared;
using System.Text.Json;

namespace Manuscrypt.UserService.Data.Repositories;

public class EventRepo
{
    private readonly UserContext _dbContext;

    public EventRepo(UserContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Event>> GetAsync(int startId, int endId)
        => await _dbContext.Events.Where(e => e.Id >= startId && e.Id <= endId).ToListAsync();

    public async Task AddCreateUserEvent(object content)
    {
        await _dbContext.Events.AddAsync(new Event
        {
            Type = EventType.CreateUser,
            OccuredAt = DateTime.UtcNow,
            ContentJson = JsonSerializer.Serialize(content)
        });
    }
    public async Task AddDeleteUserEvent(object content)
    {
        await _dbContext.Events.AddAsync(new Event
        {
            Type = EventType.DeleteUser,
            OccuredAt = DateTime.UtcNow,
            ContentJson = JsonSerializer.Serialize(content)
        });
    }
}
