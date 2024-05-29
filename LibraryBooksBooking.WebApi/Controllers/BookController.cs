using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using LibraryBooksBooking.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _bookService.GetAllAsync();
            var bookDtos = books.Select(book => new BookDTO
            {
                Guid = book.Guid,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Genre = book.Genre,
                PublishedDate = book.PublishedDate
            });

            return Ok(bookDtos);
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(string id)
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

            var bookDto = new BookDTO
            {
                Guid = book.Guid,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                Genre = book.Genre,
                PublishedDate = book.PublishedDate
            };

            return Ok(bookDto);
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook([FromBody] BookDTO bookDto)
        {
            if (string.IsNullOrEmpty(bookDto.Title) ||
                string.IsNullOrEmpty(bookDto.Author) ||
                string.IsNullOrEmpty(bookDto.ISBN) ||
                string.IsNullOrEmpty(bookDto.Genre) ||
                bookDto.PublishedDate == default)
            {
                return BadRequest("All fields are required.");
            }

            try
            {
                var book = new Book
                {
                    Guid = bookDto.Guid,
                    Title = bookDto.Title,
                    Author = bookDto.Author,
                    ISBN = bookDto.ISBN,
                    Genre = bookDto.Genre,
                    PublishedDate = (DateTime)bookDto.PublishedDate,
                    
                };

                var createdBook = await _bookService.AddAsync(book);
                var createdBookDto = new BookDTO
                {
                    Guid = createdBook.Guid,
                    Title = createdBook.Title,
                    Author = createdBook.Author,
                    ISBN = createdBook.ISBN,
                    Genre = createdBook.Genre,
                    PublishedDate = createdBook.PublishedDate
                };

                return CreatedAtAction(nameof(GetBook), new { id = createdBookDto.Guid }, createdBookDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, [FromBody] BookDTO bookDto)
        {
            if (id != bookDto.Guid)
            {
                return BadRequest("Book ID mismatch.");
            }

            var existingBook = await _bookService.GetByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound("Book not found.");
            }

            // Update only the fields that are provided in the DTO
            existingBook.Title = !string.IsNullOrEmpty(bookDto.Title) ? bookDto.Title : existingBook.Title;
            existingBook.Author = !string.IsNullOrEmpty(bookDto.Author) ? bookDto.Author : existingBook.Author;
            existingBook.ISBN = !string.IsNullOrEmpty(bookDto.ISBN) ? bookDto.ISBN : existingBook.ISBN;
            existingBook.Genre = !string.IsNullOrEmpty(bookDto.Genre) ? bookDto.Genre : existingBook.Genre;
            existingBook.PublishedDate = bookDto.PublishedDate.HasValue ? bookDto.PublishedDate.Value : existingBook.PublishedDate;

            try
            {
                await _bookService.UpdateAsync(existingBook);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
