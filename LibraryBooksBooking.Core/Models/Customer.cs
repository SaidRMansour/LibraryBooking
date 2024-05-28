using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryBooksBooking.Core.Models
{
    public class Customer
    {
        public string Guid { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        public virtual IEnumerable<Booking> Bookings { get; set; }
    }
}
