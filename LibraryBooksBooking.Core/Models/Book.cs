using System.ComponentModel.DataAnnotations;

namespace LibraryBooksBooking.Core.Models;

public class Book
{
    public string Guid { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public string Genre { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd. MMMM yyyy}", ApplyFormatInEditMode = true)]
    public DateTime PublishedDate { get; set; }
    public virtual IEnumerable<Booking> Bookings { get; set; }
}