using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Core.IRepositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
    Task<Book> GetBookByISBNAsync(string isbn);
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
}