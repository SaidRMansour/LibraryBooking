﻿namespace LibraryBooksBooking.Core.IServices;

public interface IService<T>
{
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetAsync(string id);
    Task<T> EditAsync(T entity);
    Task<T> RemoveAsync(T entity);
}