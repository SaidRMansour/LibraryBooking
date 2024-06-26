﻿using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBooksBooking.Infrastructure.Service
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepo;
        private readonly IBookService _bookService;

        public BookingService(IRepository<Booking> bookingRepo, IBookService bookService)
        {
            _bookingRepo = bookingRepo;
            _bookService = bookService;
        }

        public async Task<Booking> AddAsync(Booking entity)
        {
            return await _bookingRepo.AddAsync(entity);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _bookingRepo.GetAllAsync();
        }

        public async Task<Booking> GetByIdAsync(string id)
        {
            return await _bookingRepo.GetByIdAsync(id);
        }

        public async Task<Booking> UpdateAsync(Booking booking)
        {
            // Validate booking dates
            if (booking.BookingDate.Date < DateTime.Today)
            {
                throw new InvalidOperationException("Booking date must be today or in the future.");
            }

            if (booking.ReturnDate.Date <= booking.BookingDate.Date)
            {
                throw new InvalidOperationException("Return date must be after the booking date.");
            }
            
            var bookingExists = await _bookingRepo.GetByIdAsync(booking.Guid);
            
            if (bookingExists == null)
            {
                throw new InvalidOperationException("Booking does not exist.");
            }
            
            // Check book availability
            var availableBooks = await GetAvailableBooksAsync(booking.BookingDate, booking.ReturnDate, booking.Guid);
            var book = availableBooks.FirstOrDefault(b => b.Guid == booking.BookGuid);
            if (book == null)
            {
                throw new InvalidOperationException("Book is not available");
            }

            booking.IsAvailable = true;
            var updatedBooking = await _bookingRepo.EditAsync(booking);
            return updatedBooking;
        }
        
        public async Task<Booking> DeleteAsync(Booking entity)
        {
            return await _bookingRepo.DeleteAsync(entity);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByBookAsync(string bookGuid)
        {
            var bookings = await _bookingRepo.GetAllAsync();
            return bookings.Where(b => b.BookGuid == bookGuid);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync(DateTime start, DateTime end, string excludeBookingGuid = null)
        {
            if (start.Date > end.Date)
            {
                throw new InvalidOperationException("End date must be after the start date.");
            }
            
            var bookings = await _bookingRepo.GetAllAsync();
            var bookedBooks = bookings
                .Where(b => b.BookingDate <= end && b.ReturnDate >= start && (excludeBookingGuid == null || b.Guid != excludeBookingGuid))
                .Select(b => b.BookGuid);

            var books = await _bookService.GetAllAsync();
            return books.Where(b => !bookedBooks.Contains(b.Guid));
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            // Validate booking dates
            if (booking.BookingDate.Date < DateTime.Today)
            {
                throw new InvalidOperationException("Booking date must be today or in the future.");
            }

            if (booking.ReturnDate.Date <= booking.BookingDate.Date)
            {
                throw new InvalidOperationException("Return date must be after the booking date.");
            }

            // Check book availability
            var availableBooks = await GetAvailableBooksAsync(booking.BookingDate, booking.ReturnDate);
            var book = availableBooks.FirstOrDefault(b => b.Guid == booking.BookGuid);
            if (book == null)
            {
                throw new InvalidOperationException("Book is not available");
            }

            booking.IsAvailable = true;
            var bookingNew = await _bookingRepo.AddAsync(booking);
            return bookingNew;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerGuidAsync(string customerGuid)
        {
            if (string.IsNullOrEmpty(customerGuid))
            {
                throw new InvalidOperationException("Customer guid is required.");
            }
            
            var bookings = await _bookingRepo.GetAllAsync();
            return bookings.Where(b => b.CustomerGuid == customerGuid);
        }
    }
}
