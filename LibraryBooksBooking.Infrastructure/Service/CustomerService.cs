using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Booking> _bookingRepository;

        public CustomerService(IRepository<Customer> customerRepository, IRepository<Booking> bookingRepository)
        {
            _customerRepository = customerRepository;
            _bookingRepository = bookingRepository;
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
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Where(b => b.CustomerGuid == customerGuid);
        }
    }
}
