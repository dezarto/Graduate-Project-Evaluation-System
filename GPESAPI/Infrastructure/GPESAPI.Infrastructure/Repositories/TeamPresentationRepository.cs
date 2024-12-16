using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class TeamPresentationRepository : ITeamPresentationRepository
    {
        private readonly SqlDbContext _dbContext;

        public TeamPresentationRepository(SqlDbContext context)
        {
            _dbContext = context;
        }


        public async Task AddTeamPresentationAsync(TeamPresentation teamPresentation)
        {
            var existingTeamPresentation = await _dbContext.TeamPresentations
                .FirstOrDefaultAsync(tp => tp.TeamId == teamPresentation.TeamId);

            if (existingTeamPresentation == null)
            {
                await _dbContext.TeamPresentations.AddAsync(teamPresentation);
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<List<TeamPresentation>> GetTeamPresentationByIdAsync(int id)
        {
            return await _dbContext.TeamPresentations
                .Where(p => p.AdvisorId == id
                         || p.Professor1Id == id
                         || p.Professor2Id == id)
                .ToListAsync();
        }


        public async Task<List<TeamPresentation>> GetAllTeamPresentationsAsync()
        {
            return await _dbContext.TeamPresentations.ToListAsync();
        }

       
        public async Task UpdateTeamPresentationAsync(TeamPresentation teamPresentation)
        {
            _dbContext.TeamPresentations.Update(teamPresentation);
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteTeamPresentationAsync(int id)
        {
            var teamPresentation = (await GetTeamPresentationByIdAsync(id)).FirstOrDefault();
            if (teamPresentation != null)
            {
                _dbContext.TeamPresentations.Remove(teamPresentation);
                await _dbContext.SaveChangesAsync();
            }
        }



        public async Task<List<TeamPresentation>> GetPresentationsByDateAsync(DateTime date)
        {
            return await _dbContext.TeamPresentations
                .Where(tp => tp.PresentationDate == date)
                .ToListAsync();
        }

        
        public async Task<bool> IsPresentationSlotTakenAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return await _dbContext.TeamPresentations
                .AnyAsync(tp => tp.PresentationDate == date &&
                                ((tp.StartTime <= startTime && tp.EndTime > startTime) ||
                                 (tp.StartTime < endTime && tp.EndTime >= endTime) ||
                                 (tp.StartTime >= startTime && tp.EndTime <= endTime)));
        }

        public async Task<TeamPresentation> GetTeamPresentationByTeamIdAsync(int id)
        {
            return await _dbContext.TeamPresentations
                 .Where(p => p.TeamId == id)
                 .FirstOrDefaultAsync();
        }
    }
}
