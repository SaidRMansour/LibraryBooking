using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using LibraryBooksBooking.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBooksBooking.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                var customerDtos = customers.Select(customer => new CustomerDTO
                {
                    Guid = customer.Guid,
                    Name = customer.Name,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber
                });

                return Ok(customerDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(string id)
        {
            if (id == null)
            {
                return BadRequest("Customer ID is required.");
            }

            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                var customerDto = new CustomerDTO
                {
                    Guid = customer.Guid,
                    Name = customer.Name,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber
                };

                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> CreateCustomer([FromBody] CustomerDTO customerDto)
        {
            if (string.IsNullOrEmpty(customerDto.Name) ||
                string.IsNullOrEmpty(customerDto.Email) ||
                string.IsNullOrEmpty(customerDto.PhoneNumber))
            {
                return BadRequest("All fields are required.");
            }

            try
            {
                var customer = new Customer
                {
                    Guid = customerDto.Guid,
                    Name = customerDto.Name,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber
                };

                var createdCustomer = await _customerService.AddAsync(customer);
                var createdCustomerDto = new CustomerDTO
                {
                    Guid = createdCustomer.Guid,
                    Name = createdCustomer.Name,
                    Email = createdCustomer.Email,
                    PhoneNumber = createdCustomer.PhoneNumber
                };

                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomerDto.Guid }, createdCustomerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] CustomerDTO customerDto)
        {
            if (id != customerDto.Guid)
            {
                return BadRequest("Customer ID mismatch.");
            }

            var existingCustomer = await _customerService.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }

            // Update only the fields that are provided in the DTO
            existingCustomer.Name = !string.IsNullOrEmpty(customerDto.Name) ? customerDto.Name : existingCustomer.Name;
            existingCustomer.Email = !string.IsNullOrEmpty(customerDto.Email) ? customerDto.Email : existingCustomer.Email;
            existingCustomer.PhoneNumber = !string.IsNullOrEmpty(customerDto.PhoneNumber) ? customerDto.PhoneNumber : existingCustomer.PhoneNumber;

            try
            {
                await _customerService.UpdateAsync(existingCustomer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            if (id == null)
            {
                return BadRequest("Customer ID is required.");
            }

            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                await _customerService.DeleteAsync(customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
