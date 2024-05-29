using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryBooksBooking.WebApi.Models
{
    public class BookDTO
    {
        public string Guid { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        [RegularExpression(@"^(97(8|9))?\d{9}(\d|X)$", ErrorMessage = "Invalid ISBN format. ISBN should be either 10 or 13 digits.")]
        public string ISBN { get; set; }

        public string Genre { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PublishedDate { get; set; }
    }
}
