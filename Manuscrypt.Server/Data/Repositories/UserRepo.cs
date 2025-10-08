using Manuscrypt.Server.Data.Models;

namespace Manuscrypt.Server.Data.Repositories;

public class UserRepo : IRepo<User>
{
    private readonly ManuscryptContext _dbContext;

    public UserRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> FindByIdAsync(int id) => await _dbContext.Users.FindAsync(id);
    public async Task AddAsync(User entity) => await _dbContext.AddAsync(entity);
    public void Update(User entity) => _dbContext.Update(entity);
    public void Delete(User entity) => _dbContext.Remove(entity);
}
