using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class ProfessorsUsersRepository : IProfessorsUsersRepository
    {
        private readonly SqlDbContext _context;

        public ProfessorsUsersRepository(SqlDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProfessorsUsers professorUser)
        {
            await _context.ProfessorsUsers.AddAsync(professorUser);
            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> GetUserIdsByProfessorIdAsync(int professorId)
        {
            return await _context.ProfessorsUsers
                .Where(pu => pu.ProfessorId == professorId)
                .Select(pu => pu.UserId)
                .ToListAsync();
        }

        public async Task RemoveAsync(int professorId, int userId)
        {
            var professorUser = await _context.ProfessorsUsers
                .FirstOrDefaultAsync(pu => pu.ProfessorId == professorId && pu.UserId == userId);

            if (professorUser != null)
            {
                _context.ProfessorsUsers.Remove(professorUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int professorId, int userId)
        {
            return await _context.ProfessorsUsers
                .AnyAsync(pu => pu.ProfessorId == professorId && pu.UserId == userId);
        }
    }
}
