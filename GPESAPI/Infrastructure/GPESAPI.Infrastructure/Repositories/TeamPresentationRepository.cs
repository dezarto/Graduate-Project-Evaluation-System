using GPESAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class TeamPresentationRepository
    {
        private readonly SqlDbContext _dbContext;

        public TeamPresentationRepository(SqlDbContext context)
        {
            _dbContext = context;
        }

        // Add a new TeamPresentation record
        public async Task AddTeamPresentationAsync(TeamPresentation teamPresentation)
        {
            _dbContext.TeamPresentations.Add(teamPresentation);
            await _dbContext.SaveChangesAsync();
        }

        // Get a single TeamPresentation by ID
        public async Task<TeamPresentation> GetTeamPresentationByIdAsync(int id)
        {
            return await _dbContext.TeamPresentations
                .FirstOrDefaultAsync(tp => tp.TeamPresentationId == id);
        }

        // Get all TeamPresentations
        public async Task<List<TeamPresentation>> GetAllTeamPresentationsAsync()
        {
            return await _dbContext.TeamPresentations.ToListAsync();
        }

        // Update an existing TeamPresentation
        public async Task UpdateTeamPresentationAsync(TeamPresentation teamPresentation)
        {
            _dbContext.TeamPresentations.Update(teamPresentation);
            await _dbContext.SaveChangesAsync();
        }

        // Delete a TeamPresentation by ID
        public async Task DeleteTeamPresentationAsync(int id)
        {
            var teamPresentation = await GetTeamPresentationByIdAsync(id);
            if (teamPresentation != null)
            {
                _dbContext.TeamPresentations.Remove(teamPresentation);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Get presentations for a specific date
        public async Task<List<TeamPresentation>> GetPresentationsByDateAsync(DateTime date)
        {
            return await _dbContext.TeamPresentations
                .Where(tp => tp.PresentationDate == date)
                .ToListAsync();
        }

        // Check if a presentation slot is already taken
        public async Task<bool> IsPresentationSlotTakenAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return await _dbContext.TeamPresentations
                .AnyAsync(tp => tp.PresentationDate == date &&
                                ((tp.StartTime <= startTime && tp.EndTime > startTime) ||
                                 (tp.StartTime < endTime && tp.EndTime >= endTime) ||
                                 (tp.StartTime >= startTime && tp.EndTime <= endTime)));
        }
    }
}
