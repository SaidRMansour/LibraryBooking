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
                BookGuid = GetBooks()[0].Guid,
                CustomerGuid = GetCustomer().Guid
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
                BookGuid = GetBooks()[0].Guid,
                CustomerGuid = GetCustomer().Guid
            };
    
            
            await Assert.ThrowsAsync<InvalidOperationException>(async () => 
                await _bookingService.CreateBookingAsync(booking));
     }
     
     [Theory]
     [InlineData(4, 8,  0)]
     [InlineData(1, 10, 1)]
     public async void CreateBookingAsync_ValidBooking_ReturnsBooking(int start, int end, int book)
     {
            // Arrange
            var booking = new Booking
            {
                Guid = Guid.NewGuid().ToString(),
                BookingDate = DateTime.Today.AddDays(start),
                ReturnDate = DateTime.Today.AddDays(end),
                BookGuid = GetBooks()[book].Guid,
                CustomerGuid = GetCustomer().Guid
            };
            
            // Act
            var bookingCreated = await _bookingService.CreateBookingAsync(booking);
    
            // Assert
            Assert.NotNull(bookingCreated);
     }
     
     

     private static Customer GetCustomer()
     {
         return new Customer
         {
             Guid = "a00dfad3-8aa6-4204-897f-2c03a0d89aab",
             Name = "John Doe",
             Bookings = new List<Booking>(),
             Email = "john.doe@mail.com",
             PhoneNumber = "1234567890"
         };
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
            return new List<Booking>
            {
                new Booking
                {
                    Guid = "b84ef9ce-376b-45aa-aa28-7c97d60d8f52",
                    BookingDate = DateTime.Today.AddDays(2),
                    ReturnDate = DateTime.Today.AddDays(3),
                    BookGuid = GetBooks()[0].Guid,
                    CustomerGuid = GetCustomer().Guid
                }
            };
     }
}