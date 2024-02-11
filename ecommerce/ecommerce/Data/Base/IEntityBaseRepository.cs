using ecommerce.Models;
using System.Linq.Expressions;

namespace ecommerce.Data.Base
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        // Metoda pentru a obtine toate instantele de tipul T din DB
        Task<IEnumerable<T>> GetAllAsync();      

        // Metoda pentru a obtine toate instantele de tipul T din DB 
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        
        // Metoda care returneaza o entitate de tipul T bazata pe ID
        Task<T> GetByIdAsync(int id);            

        // Metoda care adauga o noua entitate de tipul T in DB si salveaza schimbarile
        Task AddAsync(T entity);                 

        // Metoda care actualizeaza o entitate de tipul T din DB, dupa ID, si salveaza schimbarile
        Task UpdateAsync(int id, T entity);      

        // Metoda care sterge o entitate de tipul T din DB, dupa ID, si salveaza modificarile
        Task DeleteAsync(int id);                
    }
}
