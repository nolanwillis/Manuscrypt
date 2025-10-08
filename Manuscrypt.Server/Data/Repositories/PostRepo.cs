using Manuscrypt.Server.Data.Models;

namespace Manuscrypt.Server.Data.Repositories;

public class PostRepo : IRepo<Post>
{
    private readonly ManuscryptContext _dbContext;

    public PostRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Post?> FindByIdAsync(int id) => await _dbContext.Posts.FindAsync(id);
    public async Task AddAsync(Post entity) => await _dbContext.AddAsync(entity);
    public void Update(Post entity) =>_dbContext.Update(entity);
    public void Delete(Post entity) => _dbContext.Remove(entity);
}
