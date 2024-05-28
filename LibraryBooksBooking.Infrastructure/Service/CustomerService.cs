using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBooksBooking.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IBookingService _bookingService;

        public CustomerService(IRepository<Customer> customerRepository, IBookingService bookingService)
        {
            _customerRepository = customerRepository;
            _bookingService = bookingService;
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            return await _customerRepository.AddAsync(entity);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer> GetByIdAsync(string id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer> UpdateAsync(Customer entity)
        {
            return await _customerRepository.EditAsync(entity);
        }

        public async Task<Customer> DeleteAsync(Customer entity)
        {
            return await _customerRepository.DeleteAsync(entity);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.FirstOrDefault(c => c.Email == email);
        }

        public async Task<IEnumerable<Booking>> GetCustomerBookingsAsync(string customerGuid)
        {
            return await _bookingService.GetBookingsByCustomerGuidAsync(customerGuid);
        }
    }
}
