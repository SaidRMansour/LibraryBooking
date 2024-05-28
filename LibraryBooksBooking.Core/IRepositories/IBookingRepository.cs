using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Core.IRepositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByBookAsync(string bookGuid);
    Task<IEnumerable<Book>> GetAvailableBooksAsync(DateTime start, DateTime end);
    Task<IEnumerable<Booking>> GetBookingsByCustomerGuidAsync(string customerGuid);
}