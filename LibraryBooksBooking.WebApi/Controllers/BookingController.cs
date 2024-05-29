using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryBooksBooking.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ICustomerService _customerService;
        private readonly IBookService _bookService;

        public BookingController(IBookingService bookingService, ICustomerService customerService, IBookService bookService)
        {
            _bookingService = bookingService;
            _customerService = customerService;
            _bookService = bookService;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(string id)
        {
            if (id == null)
            {
                return BadRequest("Booking ID is required.");
            }

            try
            {
                var booking = await _bookingService.GetByIdAsync(id);
                if (booking == null)
                {
                    return NotFound();
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking([FromBody] Booking booking)
        {
            if (booking.BookingDate == default ||
                booking.ReturnDate == default ||
                string.IsNullOrEmpty(booking.CustomerGuid) ||
                string.IsNullOrEmpty(booking.BookGuid))
            {
                return BadRequest("All fields are required.");
            }

            try
            {
                var success = await _bookingService.CreateBookingAsync(booking);
                if (success.IsAvailable)
                {
                    return CreatedAtAction(nameof(GetBooking), new { id = success.Guid }, success);
                }

                return BadRequest("The booking could not be created. The selected book might not be available.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, [FromBody] Booking booking)
        {
            if (id != booking.Guid)
            {
                return BadRequest("Booking ID mismatch.");
            }

            try
            {
                await _bookingService.UpdateAsync(booking);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (await _bookingService.GetByIdAsync(booking.Guid) == null)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(string id)
        {
            if (id == null)
            {
                return BadRequest("Booking ID is required.");
            }

            try
            {
                var booking = await _bookingService.GetByIdAsync(id);
                if (booking == null)
                {
                    return NotFound();
                }

                await _bookingService.DeleteAsync(booking);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
