using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBooksBooking.Infrastructure.EfCore.Repositories;

public class CustomerRepository : IRepository<Customer>
{
    private readonly LibraryBooksDbContext _context;

    public CustomerRepository(LibraryBooksDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> AddAsync(Customer entity)
    {
        await _context.Customers.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetByIdAsync(string id)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Guid == id);
    }

    public async Task<Customer> EditAsync(Customer entity)
    {
        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Customer> DeleteAsync(Customer entity)
    {
        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}