using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly SqlDbContext _dbContext;

        public TeamRepository(SqlDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddTeamAsync(Team team)
        {
            await _dbContext.Teams.AddAsync(team);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTeamAsync(int id)
        {
            var team = await GetTeamByIdAsync(id);
            if (team != null)
            {
                _dbContext.Teams.Remove(team);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _dbContext.Teams.ToListAsync();
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            return await _dbContext.Teams.FindAsync(id);
        }

        public async Task UpdateTeamAsync(Team team)
        {
            var existingTeam = await _dbContext.Teams.FindAsync(team.TeamId);

            if (existingTeam != null)
            {
                _dbContext.Entry(existingTeam).CurrentValues.SetValues(team);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Team>> GetByAdvisorIdAsync(int advisorId)
        {
            return await _dbContext.Teams
                .Where(t => t.AdvisorId == advisorId)
                .ToListAsync();
        }
    }
}
