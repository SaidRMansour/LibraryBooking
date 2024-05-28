namespace LibraryBooksBooking.Infrastructure.EfCore.DbInitializer;

public interface IDbInitializer<T>
{
    void Initialize(T context);
}