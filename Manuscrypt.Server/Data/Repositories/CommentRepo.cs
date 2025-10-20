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

    public async Task<Comment?> GetAsync(int id) => await _dbContext.Comments.FindAsync(id);
    public async Task AddAsync(Comment entity) => await _dbContext.AddAsync(entity);
    public void Update(Comment entity) => _dbContext.Update(entity);
    public void Delete(Comment entity) => _dbContext.Remove(entity);

    public async Task<IEnumerable<Comment>> GetAllAsync() => await _dbContext.Comments.ToListAsync();
}
