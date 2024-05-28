using LibraryBooksBooking.Core.IRepositories;
using LibraryBooksBooking.Core.IServices;
using LibraryBooksBooking.Core.Models;
using LibraryBooksBooking.Infrastructure.EfCore;
using LibraryBooksBooking.Infrastructure.EfCore.DbInitializer;
using LibraryBooksBooking.Infrastructure.EfCore.Repositories;
using LibraryBooksBooking.Infrastructure.Service;
using LibraryBooksBooking.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LibraryBooksDbContext>(opt => opt.UseSqlite("Data Source=LibraryBooksDB.db"));
builder.Services.AddTransient<IDbInitializer<LibraryBooksDbContext>, DbInitializer>();

builder.Services.AddScoped<IRepository<Booking>, BookingRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IRepository<Book>, BookRepository>();

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<LibraryBooksDbContext>();
    var dbInitializer = services.GetService<IDbInitializer<LibraryBooksDbContext>>();
    dbInitializer.Initialize(dbContext);
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

