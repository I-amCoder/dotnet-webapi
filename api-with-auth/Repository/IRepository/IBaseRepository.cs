﻿using System.Linq.Expressions;

namespace api_with_auth.Repository.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool tracked = true, string? includeProperties =null);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);

        Task SaveAsync();


    }
}
