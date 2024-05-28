using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryBooksBooking.Core.Models
{
    public class Book
    {
        public string Guid { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [RegularExpression(@"^\d{9}(\d|X)$|^(978|979)\d{10}$", ErrorMessage = "Invalid ISBN format. ISBN should be either 10 or 13 digits.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        public string Genre { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd. MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set; }

        public virtual IEnumerable<Booking> Bookings { get; set; }
    }
}
