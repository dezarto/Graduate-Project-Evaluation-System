using GPESAPI.Domain.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var teamPresentations = await _dbContext.TeamPresentations
                            .Where(tp => tp.TeamId == id)
                            .ToListAsync();
                        if (teamPresentations.Any())
                        {
                            _dbContext.TeamPresentations.RemoveRange(teamPresentations);
                        }

                        var teamMembers = await _dbContext.TeamMembers
                            .Where(tm => tm.TeamId == id)
                            .ToListAsync();
                        if (teamMembers.Any())
                        {
                            _dbContext.TeamMembers.RemoveRange(teamMembers);
                        }

                        var reports = await _dbContext.Reports
                            .Where(r => r.TeamId == id)
                            .ToListAsync();
                        if (reports.Any())
                        {
                            _dbContext.Reports.RemoveRange(reports);
                        }

                        var evaluations = await _dbContext.Evaluations
                            .Where(e => e.TeamId == id)
                            .ToListAsync();

                        foreach (var evaluation in evaluations)
                        {
                            var evaluationCriteriaDetails = await _dbContext.EvaluationCriteriaDetails
                                .Where(ecr => ecr.EvaluationId == evaluation.EvaluationId)
                                .ToListAsync();
                            if (evaluationCriteriaDetails.Any())
                            {
                                _dbContext.EvaluationCriteriaDetails.RemoveRange(evaluationCriteriaDetails);
                            }

                            var checklistItemDetails = await _dbContext.ChecklistItemDetails
                                .Where(c => c.EvaluationId == evaluation.EvaluationId)
                                .ToListAsync();
                            if (checklistItemDetails.Any())
                            {
                                _dbContext.ChecklistItemDetails.RemoveRange(checklistItemDetails);
                            }
                        }

                        if (evaluations.Any())
                        {
                            _dbContext.Evaluations.RemoveRange(evaluations);
                        }
                        await _dbContext.SaveChangesAsync();
                        
                        _dbContext.Teams.Remove(team);

                        await _dbContext.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();

                        throw new InvalidOperationException("An error occurred while deleting the team.", ex);
                    }
                }
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
