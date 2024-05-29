using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryBooksBooking.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(string id)
        {
            if (id == null)
            {
                return BadRequest("Book ID is required.");
            }

            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
        {
            if (string.IsNullOrEmpty(book.Title) ||
                string.IsNullOrEmpty(book.Author) ||
                string.IsNullOrEmpty(book.ISBN) ||
                string.IsNullOrEmpty(book.Genre) ||
                book.PublishedDate == default)
            {
                return BadRequest("All fields are required.");
            }

            try
            {
                var createdBook = await _bookService.AddAsync(book);
                return CreatedAtAction(nameof(GetBook), new { id = createdBook.Guid }, createdBook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, [FromBody] Book book)
        {
            if (id != book.Guid)
            {
                return BadRequest("Book ID mismatch.");
            }

            // Fjern valideringsfejl for virtuelle felter
            ModelState.Remove(nameof(book.Bookings));

            if (ModelState.IsValid)
            {
                try
                {
                    await _bookService.UpdateAsync(book);
                    return NoContent();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            if (id == null)
            {
                return BadRequest("Book ID is required.");
            }

            try
            {
                var book = await _bookService.GetByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                await _bookService.DeleteAsync(book);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}


