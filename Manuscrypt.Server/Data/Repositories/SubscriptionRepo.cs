using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data.Repositories;

public class SubscriptionRepo : IRepo<Subscription>
{
    private readonly ManuscryptContext _dbContext;

    public SubscriptionRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Subscription?> GetAsync(int id) => await _dbContext.Subscriptions.FindAsync(id);
    public async Task AddAsync(Subscription entity) => await _dbContext.AddAsync(entity);
    public void Update(Subscription entity) => _dbContext.Update(entity);
    public void Delete(Subscription entity) => _dbContext.Remove(entity);

    public async Task<IEnumerable<Subscription>> GetAllAsync() => await _dbContext.Subscriptions.ToListAsync();
}
