using Manuscrypt.Server.Data.Models;

namespace Manuscrypt.Server.Data.Repositories;

public class PostTagRepo : IRepo<PostTag>
{
    private readonly ManuscryptContext _dbContext;

    public PostTagRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PostTag?> FindByIdAsync(int id) => await _dbContext.PostTags.FindAsync(id);
    public async Task AddAsync(PostTag entity) => await _dbContext.AddAsync(entity);
    public void Update(PostTag entity) => _dbContext.Update(entity);
    public void Delete(PostTag entity) => _dbContext.Remove(entity);
}
