using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Core.IRepositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer> GetCustomerByEmailAsync(string email);
    Task<IEnumerable<Booking>> GetCustomerBookingsAsync(string customerGuid);
}