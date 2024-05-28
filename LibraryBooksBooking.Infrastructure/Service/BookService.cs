using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;

        public BookService(IRepository<Book> bookRepository)
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
            var books = await _bookRepository.GetAllAsync();
            return books.Where(b => b.Genre == genre);
        }

        public async Task<Book> GetBookByISBNAsync(string isbn)
        {
            var books = await _bookRepository.GetAllAsync();
            return books.FirstOrDefault(b => b.ISBN == isbn);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Where(b => b.Author == author);
        }
    }
}
