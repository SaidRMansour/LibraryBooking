namespace LibraryBooksBooking.Core.Models;

public class Booking
{
    public string Guid { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public bool IsAvailable { get; set; }
    public string BookGuid { get; set; }
    public string CustomerGuid { get; set; }

    public virtual Customer Customer { get; set; }
    public virtual Book Book { get; set; }
}