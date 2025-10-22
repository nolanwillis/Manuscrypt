using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data.Repositories;

public class UserRepo : IRepo<User>
{
    private readonly ManuscryptContext _dbContext;

    public UserRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<User?> GetAsync(int id) => await _dbContext.Users.FindAsync(id);
    public virtual async Task AddAsync(User entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    public virtual async Task UpdateAsync(User entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.Users.FindAsync(id);
        if (entity == null)
        {
            throw new UserDNEWithIdException(id);
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<User?> GetByEmailAsync(string email) 
        => await _dbContext.Users.FirstOrDefaultAsync(u  => u.Email == email);
    
    public virtual async Task<IEnumerable<User>> GetAllAsync(IEnumerable<int> ids)
        => await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
    
    public virtual async Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId)
        => await _dbContext.Comments.Where(c => c.UserId == userId).ToListAsync();
    
    public virtual async Task<IEnumerable<Post>> GetPostsForUserAsync(int userId)
        => await _dbContext.Posts.Where(p => p.UserId == userId).ToListAsync();
    
    public virtual async Task<IEnumerable<Subscription>> GetSubscribersForUserAsync(int userId)
        => await _dbContext.Subscriptions.Where(s => s.SubscribedToId == userId).ToListAsync();
    
    public virtual async Task<IEnumerable<Subscription>> GetSubscriptionsForUserAsync(int userId)
        => await _dbContext.Subscriptions.Where(s => s.SubscriberId  == userId).ToListAsync();
}
