using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Core.IRepositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
    Task<Book> GetBookByISBNAsync(string isbn);
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
}