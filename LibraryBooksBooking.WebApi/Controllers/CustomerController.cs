using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(string id)
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

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Name) ||
                string.IsNullOrEmpty(customer.Email) ||
                string.IsNullOrEmpty(customer.PhoneNumber))
            {
                return BadRequest("All fields are required.");
            }

            try
            {
                var createdCustomer = await _customerService.AddAsync(customer);
                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Guid }, createdCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] Customer customer)
        {
            if (id != customer.Guid)
            {
                return BadRequest("Customer ID mismatch.");
            }

            ModelState.Remove(nameof(customer.Bookings));

            if (ModelState.IsValid)
            {
                try
                {
                    await _customerService.UpdateAsync(customer);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            return BadRequest(ModelState);
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
