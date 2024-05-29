using System;
namespace LibraryBooksBooking.WebApi.Models
{
    public class BookingDTO
    {
        public string Guid { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool? IsAvailable { get; set; }
        public string BookGuid { get; set; }
        public string CustomerGuid { get; set; }
    }
}

