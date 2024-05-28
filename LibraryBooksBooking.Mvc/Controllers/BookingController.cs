using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using LibraryBooksBooking.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LibraryBooksBooking.Mvc.Controllers
{
    public class BookingController : Controller
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

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllAsync();
            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Booking/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllAsync(), "Guid", "Name");
            ViewData["BookId"] = new SelectList(await _bookService.GetAllAsync(), "Guid", "Title");
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingDate,ReturnDate,CustomerGuid,BookGuid")] Booking booking)
        {
            if (booking.BookingDate == default ||
                booking.ReturnDate == default ||
                string.IsNullOrEmpty(booking.CustomerGuid) ||
                string.IsNullOrEmpty(booking.BookGuid))
            {
                ModelState.AddModelError("", "All fields are required.");
                ViewData["CustomerId"] = new SelectList(await _customerService.GetAllAsync(), "Guid", "Name", booking.CustomerGuid);
                ViewData["BookId"] = new SelectList(await _bookService.GetAllAsync(), "Guid", "Title", booking.BookGuid);
                return View(booking);
            }

            try
            {
                var success = await _bookingService.CreateBookingAsync(booking);
                if (success.IsAvailable)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "The booking could not be created. The selected book might not be available.");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { exMessage = ex.Message });
            }

            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllAsync(), "Guid", "Name", booking.CustomerGuid);
            ViewData["BookId"] = new SelectList(await _bookService.GetAllAsync(), "Guid", "Title", booking.BookGuid);
            return View(booking);
        }



        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllAsync(), "Guid", "Name", booking.CustomerGuid);
            ViewData["BookId"] = new SelectList(await _bookService.GetAllAsync(), "Guid", "Title", booking.BookGuid);
            ViewData["BookingDate"] = booking.BookingDate.ToString("yyyy-MM-dd");
            ViewData["ReturnDate"] = booking.ReturnDate.ToString("yyyy-MM-dd");
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Guid,BookingDate,ReturnDate,IsAvailable,CustomerGuid,BookGuid")] Booking booking)
        {
            if (id != booking.Guid)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(booking.Customer));
            ModelState.Remove(nameof(booking.Book));
            if (ModelState.IsValid)
            {
                try
                {
                    await _bookingService.UpdateAsync(booking);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (await _bookingService.GetByIdAsync(booking.Guid) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return RedirectToAction("Error", new { exMessage = ex.Message });
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { exMessage = ex.Message });
                }
            }
            ViewData["CustomerId"] = new SelectList(await _customerService.GetAllAsync(), "Guid", "Name", booking.CustomerGuid);
            ViewData["BookId"] = new SelectList(await _bookService.GetAllAsync(), "Guid", "Title", booking.BookGuid);
            return View(booking);
        }


        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var booking = await _bookingService.GetByIdAsync(id);
                if (booking != null)
                {
                    await _bookingService.DeleteAsync(booking);
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
