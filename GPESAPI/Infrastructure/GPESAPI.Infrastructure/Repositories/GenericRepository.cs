using GPESAPI.Domain.Interfaces;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly SqlDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(SqlDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<T>> GetByFieldAsync<TKey>(string fieldName, TKey value)
        {
            return await _dbSet.Where(entity => EF.Property<TKey>(entity, fieldName).Equals(value)).ToListAsync();
        }
    }
}
