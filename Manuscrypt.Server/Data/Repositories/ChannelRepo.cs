using Manuscrypt.Server.Data.Models;

namespace Manuscrypt.Server.Data.Repositories;

public class ChannelRepo : IRepo<Channel>
{
    private readonly ManuscryptContext _dbContext;

    public ChannelRepo(ManuscryptContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Channel?> FindByIdAsync(int id) => await _dbContext.Channels.FindAsync(id);
    public async Task AddAsync(Channel entity) => await _dbContext.AddAsync(entity);
    public void Update(Channel entity) =>_dbContext.Update(entity);
    public void Delete(Channel entity) => _dbContext.Remove(entity);
}
