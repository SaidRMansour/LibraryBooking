using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Core.IServices;

public interface IBookingService : IService<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByBookAsync(string bookGuid);
    Task<IEnumerable<Book>> GetAvailableBooksAsync(DateTime start, DateTime end);
    Task<bool> CreateBookingAsync(Booking booking);
    Task<IEnumerable<Booking>> GetBookingsByCustomerGuidAsync(string customerGuid);
}