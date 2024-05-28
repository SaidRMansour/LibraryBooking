using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using LibraryBooksBooking.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LibraryBooksBooking.Mvc.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,ISBN,Genre,PublishedDate")] Book book)
        {
            if (string.IsNullOrEmpty(book.Title) ||
                string.IsNullOrEmpty(book.Author) ||
                string.IsNullOrEmpty(book.ISBN) ||
                string.IsNullOrEmpty(book.Genre) ||
                book.PublishedDate == default)
            {
                ModelState.AddModelError("", "All fields are required.");
                return View(book);
            }

            try
            {
                await _bookService.AddAsync(book);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { exMessage = ex.Message });
            }
        }



        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["FormattedPublishedDate"] = book.PublishedDate.ToString("yyyy-MM-dd");
            return View(book);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Guid,Title,Author,ISBN,Genre,PublishedDate")] Book book)
        {
            if (id != book.Guid)
            {
                return NotFound();
            }
            // Fjern valideringsfejl for virtuelle felter
            ModelState.Remove(nameof(book.Bookings));

            if (ModelState.IsValid)
            {
                try
                {
                    await _bookService.UpdateAsync(book);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error",new { exMessage = ex.Message});
                }
            }

            return View(book);
        }



        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var book = await _bookService.GetByIdAsync(id);
                if (book != null)
                {
                    await _bookService.DeleteAsync(book);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { exMessage = ex.Message });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string exMessage = null)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ExMessage = exMessage ?? "" });
        }
    }
}
