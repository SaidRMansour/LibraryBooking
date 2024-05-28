namespace LibraryBooksBooking.Core.IServices;

public interface IService<T>
{
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}