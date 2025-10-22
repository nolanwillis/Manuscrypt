using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data.Repositories;

public class SubscriptionRepo : IRepo<Subscription>
{
    private readonly ManuscryptContext _dbContext;

    public SubscriptionRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<Subscription?> GetAsync(int id) => await _dbContext.Subscriptions.FindAsync(id);
    public virtual async Task AddAsync(Subscription entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    public virtual async Task UpdateAsync(Subscription entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.Subscriptions.FindAsync(id);
        if (entity == null)
        {
            throw new SubscriptionDoesNotExistException(id);
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<Subscription>> GetAllAsync() => await _dbContext.Subscriptions.ToListAsync();
}
