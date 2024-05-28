using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Infrastructure.Service;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepo;

    public BookingService(IBookingRepository bookingRepo)
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
        return await _bookingRepo.GetBookingsByBookAsync(bookGuid);
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync(DateTime start, DateTime end)
    {
        return await _bookingRepo.GetAvailableBooksAsync(start, end);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByCustomerGuidAsync(string customerGuid)
    {
        return await _bookingRepo.GetBookingsByCustomerGuidAsync(customerGuid);
    }
}