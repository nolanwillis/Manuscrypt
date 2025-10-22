using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data.Repositories;

public class PostRepo : IRepo<Post>
{
    private readonly ManuscryptContext _dbContext;

    public PostRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<Post?> GetAsync(int id) => await _dbContext.Posts.FindAsync(id);
    public virtual async Task AddAsync(Post entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    public virtual async Task UpdateAsync(Post entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.Posts.FindAsync(id);
        if (entity == null)
        {
            throw new PostDoesNotExistException(id);
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<Post>> GetAllPostsAsync() => await _dbContext.Posts.ToListAsync();
    public virtual async Task<IEnumerable<Comment>> GetCommentsForPostAsync(int postId)
     => await _dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();
}
