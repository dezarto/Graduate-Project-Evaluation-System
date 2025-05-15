using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly SqlDbContext _dbContext;

        public TeamMemberRepository(SqlDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddTeamMemberAsync(TeamMember teamMember)
        {
            await _dbContext.TeamMembers.AddAsync(teamMember);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTeamMemberAsync(int teamId)
        {
            var teamMembers = await _dbContext.TeamMembers
                                       .Where(tm => tm.TeamId == teamId)
                                       .ToListAsync();

            if (teamMembers.Any())
            {
                _dbContext.TeamMembers.RemoveRange(teamMembers);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TeamMember>> GetAllTeamMembersAsync()
        {
            return await _dbContext.TeamMembers.ToListAsync();
        }

        public async Task<TeamMember> GetByUserIdAsync(int userId)
        {
            return await _dbContext.TeamMembers
                .FirstOrDefaultAsync(tm => tm.UserId == userId);
        }

        public async Task<List<TeamMember>> GetByTeamIdAsync(int teamId)
        {
            return await _dbContext.TeamMembers.Where(tm => tm.TeamId == teamId).ToListAsync();
        }

        public async Task UpdateTeamMemberAsync(TeamMember teamMember)
        {
            _dbContext.TeamMembers.Update(teamMember);
            await _dbContext.SaveChangesAsync();
        }
    }
}
