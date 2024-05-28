using System;
using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Core.IServices
{
    public interface IBookService : IService<Book>
    {
        Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
        Task<Book> GetBookByISBNAsync(string isbn);
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
    }
}
