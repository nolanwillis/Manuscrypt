namespace Manuscrypt.Server.Data.Repositories;

public interface IRepo<T> where T : class
{
    public Task<T?> GetAsync(int id);
    public Task AddAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(int id);
}
