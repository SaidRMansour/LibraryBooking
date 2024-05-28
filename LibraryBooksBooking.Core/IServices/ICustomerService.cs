using System;
using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Core.IServices;

public interface ICustomerService : IService<Customer>
{
    Task<Customer> GetCustomerByEmailAsync(string email);
    Task<IEnumerable<Booking>> GetCustomerBookingsAsync(string customerGuid);
}