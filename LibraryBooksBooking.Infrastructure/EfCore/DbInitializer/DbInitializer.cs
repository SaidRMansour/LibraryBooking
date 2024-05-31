using System;

namespace LibraryBooksBooking.Infrastructure.EfCore.DbInitializer
{
    public class DbInitializer : IDbInitializer<LibraryBooksDbContext>
    {
        public void Initialize(LibraryBooksDbContext context)
        {
            // Delete just the WebApi DB
            /* 
            // Kontroller miljøvariabel
            var initializeDb = Environment.GetEnvironmentVariable("INITIALIZE_DB");

            if (initializeDb == "Delete")
            {
                // *Databasen slettes kun i WebAPi projekt for Postman API test
                context.Database.EnsureDeleted();
            }*/

            // Delete and create MVC & WebApi DB
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
