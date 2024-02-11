
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ecommerce.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        // Injectarea AppDbContext
        // Permite accesul la baza dedate si seturile de entitati
        private readonly AppDbContext _context;
        public EntityBaseRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtinerea tuturor entitatilor
        // Permite extragerea tuturor instantelor unei entitati din DB
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        // Obtinerea tuturor entitatilor
        // Permite extragerea tuturor instantelor unei entitati din DB
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();

        }

        // Obtinerea unei entitati dupa ID
        // Cauta si returneaza o instanta a entitatii dupa ID
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(n => n.Id == id);
        }

        // Adaugarea unei noi entitati
        // Insereaza o noua instnta a entitatii in baza de date si salveaza schimbarile
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();            
        }

        // Actualizarea unei entitati existente
        // Actualizeaza o instanta existenta a entitatii si salveaza schimbarile
        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Stergerea unei entitati dupa ID
        // Elimina o instanta a entitatii si salveaza schimbarile
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(n => n.Id == id);
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
