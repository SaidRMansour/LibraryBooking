namespace LibraryBooksBooking.Core.IRepositories;

public interface IRepository<T>
{
    T AddAsync(T entity);
    IEnumerable<T> GetAllAsync();
    T GetAsync(string id);
    T EditAsync(T entity);
    T RemoveAsync(T entity);
}