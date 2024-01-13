using ecommerce.Models;
using System.Linq.Expressions;

namespace ecommerce.Data.Base
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        Task<IEnumerable<T>> GetAllAsync();      // Get all 
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T> GetByIdAsync(int id);            // Returns an entity
        Task AddAsync(T entity);                 // Add an entity
        Task UpdateAsync(int id, T entity);      // Update an entity
        Task DeleteAsync(int id);                // Delete an entity
    }
}
