using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data.Repositories;

public class CommentRepo : IRepo<Comment>
{
    private readonly ManuscryptContext _dbContext;

    public CommentRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Comment?> FindByIdAsync(int id) => await _dbContext.Comments.FindAsync(id);
    public async Task AddAsync(Comment entity) => await _dbContext.AddAsync(entity);
    public void Update(Comment entity) => _dbContext.Update(entity);
    public void Delete(Comment entity) => _dbContext.Remove(entity);

    public async Task<IEnumerable<Comment>> FindAllByUserIdAsync(int userId)
        => await _dbContext.Comments.Where(c => c.UserId == userId).ToListAsync();

    public async Task<IEnumerable<Comment>> FindAllByPostIdAsync(int postId)
        => await _dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();
}
