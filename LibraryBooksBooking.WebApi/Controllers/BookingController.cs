using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using LibraryBooksBooking.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllAsync();
                var bookingDtos = bookings.Select(booking => new BookingDTO
                {
                    Guid = booking.Guid,
                    BookingDate = booking.BookingDate,
                    ReturnDate = booking.ReturnDate,
                    IsAvailable = booking.IsAvailable,
                    BookGuid = booking.BookGuid,
                    CustomerGuid = booking.CustomerGuid
                });

                return Ok(bookingDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(string id)
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

                var bookingDto = new BookingDTO
                {
                    Guid = booking.Guid,
                    BookingDate = booking.BookingDate,
                    ReturnDate = booking.ReturnDate,
                    IsAvailable = booking.IsAvailable,
                    BookGuid = booking.BookGuid,
                    CustomerGuid = booking.CustomerGuid
                };

                return Ok(bookingDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Booking
        [HttpPost]
        public async Task<ActionResult<BookingDTO>> CreateBooking([FromBody] BookingDTO bookingDto)
        {
            if (bookingDto.BookingDate == default ||
                bookingDto.ReturnDate == default ||
                string.IsNullOrEmpty(bookingDto.CustomerGuid) ||
                string.IsNullOrEmpty(bookingDto.BookGuid))
            {
                return BadRequest("All fields are required.");
            }

            try
            {
                var book = await _bookService.GetByIdAsync(bookingDto.BookGuid);
                if (book == null)
                {
                    return NotFound("Book not found.");
                }

                var customer = await _customerService.GetByIdAsync(bookingDto.CustomerGuid);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var booking = new Booking
                {
                    Guid = bookingDto.Guid,
                    BookingDate = bookingDto.BookingDate,
                    ReturnDate = bookingDto.ReturnDate,
                    BookGuid = bookingDto.BookGuid,
                    CustomerGuid = bookingDto.CustomerGuid,
                    Book = book,
                    Customer = customer
                };

                var createdBooking = await _bookingService.CreateBookingAsync(booking);
                var createdBookingDto = new BookingDTO
                {
                    Guid = createdBooking.Guid,
                    BookingDate = createdBooking.BookingDate,
                    ReturnDate = createdBooking.ReturnDate,
                    IsAvailable = createdBooking.IsAvailable,
                    BookGuid = createdBooking.BookGuid,
                    CustomerGuid = createdBooking.CustomerGuid
                };

                return CreatedAtAction(nameof(GetBooking), new { id = createdBookingDto.Guid }, createdBookingDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(string id, [FromBody] BookingDTO bookingDto)
        {
            if (id != bookingDto.Guid)
            {
                return BadRequest("Booking ID mismatch.");
            }

            try
            {
                var existingBooking = await _bookingService.GetByIdAsync(id);
                if (existingBooking == null)
                {
                    return NotFound("Booking not found.");
                }

                var book = await _bookService.GetByIdAsync(bookingDto.BookGuid);
                if (book == null)
                {
                    return NotFound("Book not found.");
                }

                var customer = await _customerService.GetByIdAsync(bookingDto.CustomerGuid);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                // Update only the fields that are provided in the DTO
                existingBooking.BookingDate = bookingDto.BookingDate != default ? bookingDto.BookingDate : existingBooking.BookingDate;
                existingBooking.ReturnDate = bookingDto.ReturnDate != default ? bookingDto.ReturnDate : existingBooking.ReturnDate;
                existingBooking.BookGuid = !string.IsNullOrEmpty(bookingDto.BookGuid) ? bookingDto.BookGuid : existingBooking.BookGuid;
                existingBooking.CustomerGuid = !string.IsNullOrEmpty(bookingDto.CustomerGuid) ? bookingDto.CustomerGuid : existingBooking.CustomerGuid;
                existingBooking.Book = book;
                existingBooking.Customer = customer;

                await _bookingService.UpdateAsync(existingBooking);
                return NoContent();
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
