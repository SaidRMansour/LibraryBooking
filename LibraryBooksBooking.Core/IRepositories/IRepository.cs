namespace LibraryBooksBooking.Core.IRepositories;

public interface IRepository<T>
{
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(string id);
    Task<T> EditAsync(T entity);
    Task<T> RemoveAsync(T entity);
}