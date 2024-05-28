using LibraryBooksBooking.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooksBooking.Infrastructure.EfCore;

public class LibraryBooksDbContext(DbContextOptions<LibraryBooksDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasKey(x => x.Guid);
        modelBuilder.Entity<Customer>().HasKey(x => x.Guid);
        modelBuilder.Entity<Booking>().HasKey(x => x.Guid);
        
        modelBuilder.Entity<Customer>()
            .HasMany(x => x.Bookings)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerGuid);
        
        modelBuilder.Entity<Book>()
            .HasMany(x => x.Bookings)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookGuid);
        
        modelBuilder.Entity<Booking>()
            .HasOne(x => x.Customer)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.CustomerGuid);

        modelBuilder.Entity<Booking>()
            .HasOne(x => x.Book)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.BookGuid);
    }
}
