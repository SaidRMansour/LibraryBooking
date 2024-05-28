using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;

namespace LibraryBooksBooking.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookingRepository _bookingRepository;

        public CustomerService(ICustomerRepository customerRepository, IBookingRepository bookingRepository)
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
            return await _customerRepository.GetCustomerByEmailAsync(email);
        }

        public async Task<IEnumerable<Booking>> GetCustomerBookingsAsync(string customerGuid)
        {
            return await _customerRepository.GetCustomerBookingsAsync(customerGuid);
        }
    }
}
