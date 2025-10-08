using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data.Repositories;

public class EditRepo : IRepo<Edit>
{
    private readonly ManuscryptContext _dbContext;

    public EditRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Edit?> FindByIdAsync(int id) => await _dbContext.Edits.FindAsync(id);
    public async Task AddAsync(Edit entity)
    {
        await _dbContext.AddAsync(entity);
    }
    public void Update(Edit entity) => _dbContext.Update(entity);
    public void Delete(Edit entity) => _dbContext.Remove(entity);

    public async Task<IEnumerable<Edit>> FindAllByPostIdAsync(int postId)
        => await _dbContext.Edits.Where(e => e.PostId == postId).ToListAsync();
}
