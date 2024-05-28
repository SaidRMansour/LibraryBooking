namespace LibraryBooksBooking.Core.IServices;

public interface IService<T>
{
    T AddAsync(T entity);
    IEnumerable<T> GetAllAsync();
    T GetAsync(string id);
    T EditAsync(T entity);
    T RemoveAsync(T entity);
}