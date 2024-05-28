﻿namespace LibraryBooksBooking.Core.Models;

public class Book
{
    public string Guid { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public string Genre { get; set; }
    public DateTime PublishedDate { get; set; }
}