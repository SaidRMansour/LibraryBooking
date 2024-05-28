using LibraryBooksBooking.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooksBooking.Infrastructure.EfCore
{
    public class LibraryBooksDbContext : DbContext
    {
        public LibraryBooksDbContext(DbContextOptions<LibraryBooksDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasKey(x => x.Guid);
            modelBuilder.Entity<Customer>().HasKey(x => x.Guid);
            modelBuilder.Entity<Booking>().HasKey(x => x.Guid);

            modelBuilder.Entity<Booking>()
                .HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerGuid);

            modelBuilder.Entity<Booking>()
                .HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookGuid);
        }
    }
}
