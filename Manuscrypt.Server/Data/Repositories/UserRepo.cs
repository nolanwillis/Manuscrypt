using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data.Repositories;

public class UserRepo : IRepo<User>
{
    private readonly ManuscryptContext _dbContext;

    public UserRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetAsync(int id) => await _dbContext.Users.FindAsync(id);
    public async Task AddAsync(User entity) => await _dbContext.AddAsync(entity);
    public void Update(User entity) => _dbContext.Update(entity);
    public void Delete(User entity) => _dbContext.Remove(entity);

    public async Task<User?> GetByEmailAsync(string email) 
        => await _dbContext.Users.FirstOrDefaultAsync(u  => u.Email == email);
    
    public async Task<IEnumerable<User>> GetAllAsync(IEnumerable<int> ids)
        => await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
    
    public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId)
        => await _dbContext.Comments.Where(c => c.UserId == userId).ToListAsync();
    
    public async Task<IEnumerable<Post>> GetPostsForUserAsync(int userId)
        => await _dbContext.Posts.Where(p => p.UserId == userId).ToListAsync();
    
    public async Task<IEnumerable<Subscription>> GetSubscribersForUserAsync(int userId)
        => await _dbContext.Subscriptions.Where(s => s.SubscribedToId == userId).ToListAsync();
    
    public async Task<IEnumerable<Subscription>> GetSubscriptionsForUserAsync(int userId)
        => await _dbContext.Subscriptions.Where(s => s.SubscriberId  == userId).ToListAsync();
}
