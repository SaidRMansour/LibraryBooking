using System;
using LibraryBooksBooking.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LibraryBooksBooking.WebApi.Models
{
	public class CustomerDTO
	{
        public string Guid { get; set; }

        public string Name { get; set; }
        
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

    }
}

