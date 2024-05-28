using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Infrastructure.Service;

public class BookingService : IBookingService
{
    private readonly IRepository<Booking> _bookingRepo;

    public BookingService(IRepository<Booking> bookingRepo)
    {
        _bookingRepo = bookingRepo;
    }

    public async Task<Booking> AddAsync(Booking entity)
    {
        return await _bookingRepo.AddAsync(entity);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _bookingRepo.GetAllAsync();
    }

    public async Task<Booking> GetByIdAsync(string id)
    {
        return await _bookingRepo.GetByIdAsync(id);
    }

    public async Task<Booking> UpdateAsync(Booking entity)
    {
        return await _bookingRepo.EditAsync(entity);
    }

    public async Task<Booking> DeleteAsync(Booking entity)
    {
        return await _bookingRepo.DeleteAsync(entity);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByBookAsync(string bookGuid)
    {
        var bookings = await _bookingRepo.GetAllAsync();
        return bookings.Where(b => b.BookGuid == bookGuid);
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync(DateTime start, DateTime end)
    {
        var bookings = await _bookingRepo.GetAllAsync();
        var bookedBooks = bookings.Where(b => b.BookingDate <= end && b.ReturnDate >= start).Select(b => b.BookGuid);
        return bookings.Where(b => !bookedBooks.Contains(b.Guid)).Select(b => new Book { Guid = b.BookGuid });
    }

    public async Task<IEnumerable<Booking>> GetBookingsByCustomerGuidAsync(string customerGuid)
    {
        var bookings = await _bookingRepo.GetAllAsync();
        return bookings.Where(b => b.CustomerGuid == customerGuid);
    }
}