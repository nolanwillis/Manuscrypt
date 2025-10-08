namespace Manuscrypt.Server.Data.Repositories;

public interface IRepo<T> where T : class
{
    public Task<T?> FindByIdAsync(int id);
    public Task AddAsync(T entity);
    public void Update(T entity);
    public void Delete(T entity);
}
