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

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBookService, BookService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Initialize the database.
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<LibraryBooksDbContext>();
    var dbInitializer = services.GetService<IDbInitializer<LibraryBooksDbContext>>();
    dbInitializer.Initialize(dbContext);
}

// app.UseHttpsRedirection();

app.Run();