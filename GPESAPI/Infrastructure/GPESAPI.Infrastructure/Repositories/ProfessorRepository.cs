using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly SqlDbContext _dbContext;
        public ProfessorRepository(SqlDbContext context)
        {
            _dbContext = context;
        }
        
        public async Task<Professor> AddAsync(Professor professor)
        {
            await _dbContext.Professors.AddAsync(professor);
            await _dbContext.SaveChangesAsync();
            return professor;
        }

        public async Task DeleteAsync(int id)
        {
            var professor = await GetByIdAsync(id);
            if (professor != null)
            {
                _dbContext.Professors.Remove(professor);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Professor>> GetAllAsync()
        {
            return await _dbContext.Professors.ToListAsync();
        }

        public async Task<Professor> GetByIdAsync(int id)
        {
            return await _dbContext.Professors.FindAsync(id);
        }

        public async Task<Professor> GetByEmailAsync(string email)
        {
            return await _dbContext.Professors
                .FirstOrDefaultAsync(p => p.mailAddress == email);
        }

        public async Task UpdateAsync(Professor professor)
        {
            _dbContext.Professors.Update(professor);
            await _dbContext.SaveChangesAsync();
        }
    }
}
