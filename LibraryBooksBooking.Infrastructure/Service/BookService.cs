using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Book> AddAsync(Book entity)
    {
        return await _bookRepository.AddAsync(entity);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _bookRepository.GetAllAsync();
    }

    public async Task<Book> GetByIdAsync(string id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<Book> UpdateAsync(Book entity)
    {
        return await _bookRepository.EditAsync(entity);
    }

    public async Task<Book> DeleteAsync(Book entity)
    {
        return await _bookRepository.DeleteAsync(entity);
    }

    public async Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre)
    {
        return await _bookRepository.GetBooksByGenreAsync(genre);
    }

    public async Task<Book> GetBookByISBNAsync(string isbn)
    {
        return await _bookRepository.GetBookByISBNAsync(isbn);
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
    {
        return await _bookRepository.GetBooksByAuthorAsync(author);
    }
}