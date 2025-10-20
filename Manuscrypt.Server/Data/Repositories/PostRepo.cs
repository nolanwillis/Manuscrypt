using Manuscrypt.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Manuscrypt.Server.Data.Repositories;

public class PostRepo : IRepo<Post>
{
    private readonly ManuscryptContext _dbContext;

    public PostRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Post?> GetAsync(int id) => await _dbContext.Posts.FindAsync(id);
    public async Task AddAsync(Post entity) => await _dbContext.AddAsync(entity);
    public void Update(Post entity) =>_dbContext.Update(entity);
    public void Delete(Post entity) => _dbContext.Remove(entity);

    public async Task<IEnumerable<Post>> GetAllPostsAsync() => await _dbContext.Posts.ToListAsync();
    public async Task<IEnumerable<Comment>> GetCommentsForPostAsync(int postId)
     => await _dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();
}
