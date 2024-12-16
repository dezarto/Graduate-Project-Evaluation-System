using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlDbContext _dbContext;
        public UserRepository(SqlDbContext context)
        {
            _dbContext = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                // ProfessorsUsers tablosundaki ilişkili kaydı sil
                var professorUser = await _dbContext.ProfessorsUsers
                                                     .Where(pu => pu.UserId == id)
                                                     .ToListAsync();
                if (professorUser.Any())
                {
                    _dbContext.ProfessorsUsers.RemoveRange(professorUser);
                }

                // TeamMembers tablosundaki ilişkili kaydı sil
                var teamMembers = await _dbContext.TeamMembers
                                                   .Where(tm => tm.UserId == id)
                                                   .ToListAsync();
                if (teamMembers.Any())
                {
                    _dbContext.TeamMembers.RemoveRange(teamMembers);
                }

                // Kullanıcıyı Users tablosundan sil
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> ExistsByStudentNumberAsync(string studentNumber)
        {
            return await _dbContext.Users.AnyAsync(u => u.StudentNumber == studentNumber);
        }

        public async Task<List<User>> GetAllUsersWithProfessorsAsync()
        {
            return await _dbContext.Set<User>()
                .Where(user => user.ProfessorId != null)
                .ToListAsync();
        }

        public async Task<User> GetByStudentNumberAsync(string studentNumber)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.StudentNumber == studentNumber);
        }
    }
}
