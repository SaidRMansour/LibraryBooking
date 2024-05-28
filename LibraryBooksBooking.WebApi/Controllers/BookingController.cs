using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBooksBooking.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Booking booking)
    {
        if (booking == null)
        {
            return BadRequest();
        }

        var createdBooking = await _bookingService.CreateBookingAsync(booking);
        
        return CreatedAtAction(nameof(Get), new { id = createdBooking.Guid }, createdBooking);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> Get()
    {
        var bookings = await _bookingService.GetAllAsync();
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> Get(string id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        
        if (booking == null)
        {
            return NotFound();
        }
        
        return Ok(booking);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Booking>> Put(string id, [FromBody] Booking booking)
    {
        if (booking == null || id != booking.Guid)
        {
            return BadRequest();
        }

        var updatedBooking = await _bookingService.UpdateAsync(booking);
        if (updatedBooking == null)
        {
            return NotFound();
        }

        return Ok(updatedBooking);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        await _bookingService.DeleteAsync(booking);
        return NoContent();
    }
}