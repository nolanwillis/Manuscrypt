using Manuscrypt.Server.Data.Models;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Manuscrypt.Server.Data.Repositories;

public class CommentRepo : IRepo<Comment>
{
    private readonly ManuscryptContext _dbContext;

    public CommentRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<Comment?> GetAsync(int id) => await _dbContext.Comments.FindAsync(id);
    public virtual async Task AddAsync(Comment entity) 
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

    public virtual async Task<IEnumerable<Comment>> GetAllAsync() => await _dbContext.Comments.ToListAsync();
}
