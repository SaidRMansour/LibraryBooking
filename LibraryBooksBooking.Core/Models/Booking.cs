using System.ComponentModel.DataAnnotations;

namespace LibraryBooksBooking.Core.Models;

public class Booking
{
    public string Guid { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd. MMMM yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name ="Booking date")]
    public DateTime BookingDate { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd. MMMM yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name ="Return date")]
    public DateTime ReturnDate { get; set; }
    public bool IsAvailable { get; set; }
    [Display(Name ="Book")]
    public string BookGuid { get; set; }
    [Display(Name ="Customer")]
    public string CustomerGuid { get; set; }

    public virtual Customer Customer { get; set; }
    public virtual Book Book { get; set; }
}