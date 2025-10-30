using Manuscrypt.PostService.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.PostService.Data.Repositories;

public class PostRepo
{
    private readonly PostContext _dbContext;

    public PostRepo(PostContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<Post?> GetAsync(int postId) => await _dbContext.Posts.FindAsync(postId);

    public virtual async Task<IEnumerable<Post>> GetPostsByUserAsync(int userId)
        => await _dbContext.Posts.Where(p => p.UserId == userId).ToListAsync();

    public virtual async Task AddAsync(Post post)
    {
        await _dbContext.AddAsync(post);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(Post post)
    {
        _dbContext.Update(post);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int postId)
    {
        var entity = await _dbContext.Posts.FindAsync(postId);
        if (entity == null)
        {
            throw new PostDoesNotExistException(postId);
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
