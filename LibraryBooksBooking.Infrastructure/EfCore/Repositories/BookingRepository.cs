using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooksBooking.Infrastructure.EfCore.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly LibraryBooksDbContext _context;

    public BookingRepository(LibraryBooksDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> AddAsync(Booking entity)
    {
        await _context.Bookings.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<Booking> GetByIdAsync(string id)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.Guid == id);
    }

    public async Task<Booking> EditAsync(Booking entity)
    {
        _context.Bookings.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Booking> DeleteAsync(Booking entity)
    {
        _context.Bookings.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<Booking>> GetBookingsByBookAsync(string bookGuid)
    {
        return await _context.Bookings.Where(b => b.BookGuid == bookGuid).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync(DateTime start, DateTime end)
    {
        var bookings = await _context.Bookings.Where(b => b.BookingDate >= start && b.ReturnDate <= end).ToListAsync();
        
        return await _context.Books.Where(b => !bookings.Select(b => b.BookGuid).Contains(b.Guid)).ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByCustomerGuidAsync(string customerGuid)
    {
        return await _context.Bookings.Where(b => b.CustomerGuid == customerGuid).ToListAsync();
    }
}