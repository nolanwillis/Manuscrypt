using Manuscrypt.CommentService.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.CommentService.Data.Repositories;

public class CommentRepo
{
    private readonly CommentContext _dbContext;

    public CommentRepo(CommentContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<Comment?> GetAsync(int id) => await _dbContext.Comments.FindAsync(id);

    public virtual async Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId)
        => await _dbContext.Comments.Where(c => c.UserId == userId).ToListAsync();

    public virtual async Task<IEnumerable<Comment>> GetCommentsForPostAsync(int postId)
        => await _dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();

    public virtual async Task CreateAsync(Comment entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }
    
    public virtual async Task UpdateAsync(Comment entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
    }
    
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbContext.Comments.FindAsync(id);
        if (entity == null)
        {
            throw new CommentDoesNotExistException(id);
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
