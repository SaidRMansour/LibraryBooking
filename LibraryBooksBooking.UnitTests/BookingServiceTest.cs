using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using LibraryBooksBooking.Infrastructure.Service;
using Moq;
using Xunit.Abstractions;

namespace LibraryBooksBooking.UnitTests;

public class BookingServiceTest
{ 
    private readonly ITestOutputHelper _testOutputHelper;
    private IBookingService _bookingService;
    private Mock<IRepository<Booking>> _bookingRepository;
    private Mock<IBookService> _bookService;

    public BookingServiceTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _bookingRepository = new Mock<IRepository<Booking>>();
        _bookService = new Mock<IBookService>();
        
        _bookingRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(GetBookings());
        
        _bookService.Setup(x => x.GetAllAsync()).ReturnsAsync(GetBooks());
        
        _bookingRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))!.ReturnsAsync((string id) => 
            GetBookings().FirstOrDefault(b => b.Guid == id));
        
        _bookingRepository.Setup(x => x.AddAsync(It.IsAny<Booking>())).ReturnsAsync((Booking booking) =>
        {
            GetBookings().Add(booking);
            return booking;
        });
        
        _bookingRepository.Setup(x => x.EditAsync(It.IsAny<Booking>())).ReturnsAsync((Booking booking) =>
        {
            var bookingToUpdate = GetBookings().FirstOrDefault(b => b.Guid == booking.Guid);
            bookingToUpdate = booking;
            return bookingToUpdate;
        });
        
        _bookingService = new BookingService(_bookingRepository.Object, _bookService.Object);
    }
    
    [Theory]
    [InlineData(-2, -1)]
    [InlineData(-2, 2)]
    public async void CreateBookingAsync_BookingDateInThePast_ThrowsInvalidOperationException(int start, int end)
    {
        // Arrange
        var booking = new Booking
        {
            Guid = Guid.NewGuid().ToString(),
            BookingDate = DateTime.Today.AddDays(start),
            ReturnDate = DateTime.Today.AddDays(end),
            BookGuid = GetBooks().First().Guid,
            CustomerGuid = GetCustomers().First().Guid
        };

        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await _bookingService.CreateBookingAsync(booking));
    }
     
    [Theory]
    [InlineData(2, 1)]
    [InlineData(10, 0)]
    public async void CreateBookingAsync_ReturnDateBeforeBookingDate_ThrowsInvalidOperationException(int start, int end)
    {
            // Arrange
        var booking = new Booking
        {
            Guid = Guid.NewGuid().ToString(),
            BookingDate = DateTime.Today.AddDays(start),
            ReturnDate = DateTime.Today.AddDays(end),
            BookGuid = GetBooks().First().Guid,
            CustomerGuid = GetCustomers().First().Guid
        };

            
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await _bookingService.CreateBookingAsync(booking));
     }
     
    [Theory]
    [InlineData(4, 8,  0)]
    [InlineData(6, 10, 1)]
    public async void CreateBookingAsync_ValidBooking_ReturnsBooking(int start, int end, int book)
    {
        // Arrange
        var booking = new Booking
        {
            Guid = Guid.NewGuid().ToString(),
            BookingDate = DateTime.Today.AddDays(start),
            ReturnDate = DateTime.Today.AddDays(end),
            BookGuid = GetBooks()[book].Guid,
            CustomerGuid = GetCustomers()[0].Guid
        };
        
        // Act
        var bookingCreated = await _bookingService.CreateBookingAsync(booking);

        // Assert
        Assert.NotNull(bookingCreated);
     }
     
    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    public async void CreateBookingAsync_BookNotAvailable_ThrowsInvalidOperationException(int start, int end)
    {
       // Arrange
       var booking = new Booking
       {
           Guid = Guid.NewGuid().ToString(),
           BookingDate = DateTime.Today.AddDays(start),
           ReturnDate = DateTime.Today.AddDays(end),
           BookGuid = GetBooks()[0].Guid,
           CustomerGuid = GetCustomers()[0].Guid
       };

       // Act & Assert
       await Assert.ThrowsAsync<InvalidOperationException>(async () => 
           await _bookingService.CreateBookingAsync(booking));
    }

    [Theory]
    [InlineData(1, 2, 0)]
    [InlineData(4, 5, 1)]
    [InlineData(2, 5, 0)]
    public async void GetAvailableBooksAsync_BookingsExist_ReturnsAvailableBooks(int start, int end, int amount)
    { 
        // Arrange
        var startDate = DateTime.Today.AddDays(start);
        var endDate = DateTime.Today.AddDays(end);
       
        // Act
        var availableBooks = 
            await _bookingService.GetAvailableBooksAsync(startDate, endDate, null);
       
        // Assert
        Assert.Equal(amount, availableBooks.ToList().Count);
    }
    
    [Theory]
    [InlineData(3, 2)]
    [InlineData(0, -3)]
    public async void GetAvailableBooksAsync_EndDateBeforeStartDate_ThrowsInvalidOperationException(int start, int end)
    {
        // Arrange
        var startDate = DateTime.Today.AddDays(start);
        var endDate = DateTime.Today.AddDays(end);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            await _bookingService.GetAvailableBooksAsync(startDate, endDate, null));
    }
    
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public async void GetBookingsByBookAsync_BookingsExist_ReturnsBookings(int book, int amount)
    {
        // Arrange
        var bookGuid = GetBooks()[book].Guid;
        
        // Act
        var bookings = await _bookingService.GetBookingsByBookAsync(bookGuid);
        
        // Assert
        Assert.Equal(amount, bookings.ToList().Count);
    }
    
    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 1)]
    public async void GetBookingsByCustomerGuidAsync_BookingsExist_ReturnsBookings(int customer, int amount)
    {
        // Arrange
        var customerGuid = GetCustomers()[customer].Guid;
        
        // Act
        var bookings = await _bookingService.GetBookingsByCustomerGuidAsync(customerGuid);
        
        // Assert
        Assert.Equal(amount, bookings.ToList().Count);
    }
    
    [Fact]
    public async void GetBookingsByCustomerGuidAsync_NoBookings_ReturnsEmptyList()
    {
        // Arrange
        // Generate a random customer guid that does not exist
        var customerGuid = Guid.NewGuid().ToString();
        
        // Act
        var bookings = await _bookingService.GetBookingsByCustomerGuidAsync(customerGuid);
        
        // Assert
        Assert.Empty(bookings);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void GetBookingsByCustomerGuidAsync_InvalidCustomerGuid_ReturnsEmptyList(string customerGuid)
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _bookingService.GetBookingsByCustomerGuidAsync(customerGuid));
    }
    
    [Fact]
    public async void UpdateBookingAsync_BookingExists_ReturnsUpdatedBooking()
    {
        // Arrange
        var booking = GetBookings().First();
        
        booking.BookingDate = DateTime.Today.AddDays(5);
        booking.ReturnDate = DateTime.Today.AddDays(10);
        
        // Act
        var bookingUpdated = await _bookingService.UpdateAsync(booking);
        
        // Assert
        Assert.Equal(booking.ReturnDate, bookingUpdated.ReturnDate);
    }
    
    [Fact]
    public void UpdateBookingAsync_BookingDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var booking = new Booking
        {
            Guid = Guid.NewGuid().ToString(),
            BookingDate = DateTime.Today.AddDays(10),
            ReturnDate = DateTime.Today.AddDays(12),
            BookGuid = GetBooks().First().Guid,
            CustomerGuid = GetCustomers().First().Guid
        };
        
        
        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _bookingService.UpdateAsync(booking));
    }
    
    [Theory]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    public async void UpdateBookingAsync_ReturnDateBeforeBookingDate_ThrowsInvalidOperationException(int start, int end)
    {
        // Arrange
        var booking = GetBookings().First();
        booking.BookingDate = DateTime.Today.AddDays(start);
        booking.ReturnDate = DateTime.Today.AddDays(end);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _bookingService.UpdateAsync(booking));
    }
    
    [Theory]
    [InlineData(-1, -1)]
    [InlineData(-20, -16)]
    public async void UpdateBookingAsync_BookingDateInThePast_ThrowsInvalidOperationException(int start, int end)
    {
        // Arrange
        var booking = GetBookings().First();
        booking.BookingDate = DateTime.Today.AddDays(start);
        booking.BookingDate = DateTime.Today.AddDays(end);
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _bookingService.UpdateAsync(booking));
    }
    
    [Fact]
    public async void UpdateBookingAsync_BookNotAvailable_ThrowsInvalidOperationException()
    {
        // Arrange
        var booking = GetBookings().First();
        booking.BookGuid = GetBooks().Last().Guid;
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await _bookingService.UpdateAsync(booking));
    }
    
    private static List<Customer> GetCustomers()
    {
        return
        [
            new Customer
            { 
                Guid = "a00dfad3-8aa6-4204-897f-2c03a0d89aab",
                Name = "John Doe",
                Bookings = new List<Booking>(),
                Email = "john.doe@mail.com",
                PhoneNumber = "1234567890"
            }, 
            new Customer 
            { 
                Guid = "cdf7d4d8-f1d4-4869-b17c-7db3b9daa90e", 
                Name = "Jane Doe", 
                Bookings = new List<Booking>(), 
                Email = "jane.doe@mail.com", 
                PhoneNumber = "1234567891" 
            }
        ];
    }
     
    private static List<Book> GetBooks()
    {
        return
        [
            new Book
            {
                Guid = "1092fcf8-dddf-4bb7-b7e0-a526f9d884c1",
                Title = "Book 1",
                Author = "Author 1",
                ISBN = "1234567890"
             },
            new Book
            {
                Guid = "f83b8dbc-53a8-4e3a-a544-dd35e8eb8c24",
                Title = "Book 2",
                Author = "Author 2",
                ISBN = "1234567891"
            }
        ]; 
    }
     
     private static List<Booking> GetBookings()
     {
         return
         [
             new Booking
             {
                 Guid = "b84ef9ce-376b-45aa-aa28-7c97d60d8f52",
                 BookingDate = DateTime.Today.AddDays(2),
                 ReturnDate = DateTime.Today.AddDays(3),
                 BookGuid = GetBooks().First().Guid,
                 CustomerGuid = GetCustomers().First().Guid
             },
             new Booking
             {
                 Guid = "580c0968-6d7f-44a7-bc25-908fa4a5c1aa",
                 BookingDate = DateTime.Today.AddDays(-2),
                 ReturnDate = DateTime.Today.AddDays(3),
                 BookGuid = GetBooks().Last().Guid,
                 CustomerGuid = GetCustomers().Last().Guid
             },
             new Booking
             {
                 Guid = "240df115-7943-4528-9eb0-d013cba41332",
                 BookingDate = DateTime.Today.AddDays(4),
                 ReturnDate = DateTime.Today.AddDays(5),
                 BookGuid = GetBooks().Last().Guid,
                 CustomerGuid = GetCustomers().First().Guid
             }
         ]; 
     }
}