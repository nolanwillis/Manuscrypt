using Manuscrypt.UserService.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Manuscrypt.UserService.Data.Repositories;

public class UserRepo
{
    private readonly UserContext _dbContext;

    public UserRepo(UserContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<User?> GetAsync(int userId) => await _dbContext.Users.FindAsync(userId);

    public virtual async Task CreateAsync(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(User user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int userId)
    {
        var entity = await _dbContext.Users.FindAsync(userId);
        if (entity == null)
        {
            throw new UserDNEWithIdException(userId);
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<int> userIds)
        => await _dbContext.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();

    public virtual async Task<User?> GetByEmailAsync(string email)
        => await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
}
