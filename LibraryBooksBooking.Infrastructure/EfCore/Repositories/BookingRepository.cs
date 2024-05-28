using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooksBooking.Infrastructure.EfCore.Repositories;

public class BookingRepository : IRepository<Booking>
{
    private readonly LibraryBooksDbContext _context;

    public BookingRepository(LibraryBooksDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> AddAsync(Booking entity)
    {
        entity.Guid = Guid.NewGuid().ToString();
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
}