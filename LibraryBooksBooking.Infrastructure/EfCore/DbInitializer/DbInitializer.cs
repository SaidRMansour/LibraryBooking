namespace LibraryBooksBooking.Infrastructure.EfCore.DbInitializer;

public class DbInitializer : IDbInitializer<LibraryBooksDbContext>
{
    public void Initialize(LibraryBooksDbContext context)
    {
        //context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}