using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Core.IServices;

public interface IBookingService
{
    IEnumerable<Booking> GetBookingsByBookAsync(string bookGuid);
    IEnumerable<Book> GetAvailableBooksAsync(DateTime start, DateTime end);
    IEnumerable<Book> GetBookingsByCustomerGuidAsync(string customerGuid);
}