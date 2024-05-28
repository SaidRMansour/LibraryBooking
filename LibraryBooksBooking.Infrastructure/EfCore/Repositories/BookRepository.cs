using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooksBooking.Infrastructure.EfCore.Repositories;

public class BookRepository : IRepository<Book>
{
    private readonly LibraryBooksDbContext _context;

    public BookRepository(LibraryBooksDbContext context)
    {
        _context = context;
    }

    public async Task<Book> AddAsync(Book entity)
    {
        entity.Guid = Guid.NewGuid().ToString();
        await _context.Books.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetByIdAsync(string id)
    {
        return await _context.Books.FirstOrDefaultAsync(b => b.Guid == id);
    }

    public async Task<Book> EditAsync(Book entity)
    {
        _context.Books.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Book> DeleteAsync(Book entity)
    {
        _context.Books.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}