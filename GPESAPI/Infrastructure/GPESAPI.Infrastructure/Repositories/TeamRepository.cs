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
                        // 1. İlgili TeamPresentation kayıtlarını sil
                        var teamPresentations = await _dbContext.TeamPresentations
                            .Where(tp => tp.TeamId == id)
                            .ToListAsync();
                        if (teamPresentations.Any())
                        {
                            _dbContext.TeamPresentations.RemoveRange(teamPresentations);
                        }

                        // 2. İlgili TeamMembers kayıtlarını sil
                        var teamMembers = await _dbContext.TeamMembers
                            .Where(tm => tm.TeamId == id)
                            .ToListAsync();
                        if (teamMembers.Any())
                        {
                            _dbContext.TeamMembers.RemoveRange(teamMembers);
                        }

                        // 3. İlgili Reports kayıtlarını sil
                        var reports = await _dbContext.Reports
                            .Where(r => r.TeamId == id)
                            .ToListAsync();
                        if (reports.Any())
                        {
                            _dbContext.Reports.RemoveRange(reports);
                        }

                        // 4. İlgili Evaluations kayıtlarını sil
                        var evaluations = await _dbContext.Evaluations
                            .Where(e => e.TeamId == id)
                            .ToListAsync();

                        foreach (var evaluation in evaluations)
                        {
                            // 4.1. İlgili EvaluationCriteriaDetails kayıtlarını sil
                            var evaluationCriteriaDetails = await _dbContext.EvaluationCriteriaDetails
                                .Where(ecr => ecr.EvaluationId == evaluation.EvaluationId)
                                .ToListAsync();
                            if (evaluationCriteriaDetails.Any())
                            {
                                _dbContext.EvaluationCriteriaDetails.RemoveRange(evaluationCriteriaDetails);
                            }

                            // 4.2. İlgili ChecklistItemDetails kayıtlarını sil
                            var checklistItemDetails = await _dbContext.ChecklistItemDetails
                                .Where(c => c.EvaluationId == evaluation.EvaluationId)
                                .ToListAsync();
                            if (checklistItemDetails.Any())
                            {
                                _dbContext.ChecklistItemDetails.RemoveRange(checklistItemDetails);
                            }
                        }

                        // 5. Evaluations tablosundaki kayıtları sil
                        if (evaluations.Any())
                        {
                            _dbContext.Evaluations.RemoveRange(evaluations);
                        }

                        // 6. Team kaydını sil
                        _dbContext.Teams.Remove(team);

                        // 7. Değişiklikleri kaydet
                        await _dbContext.SaveChangesAsync();

                        // Eğer her şey başarılıysa işlemi onayla
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        // Hata durumunda işlemi geri al
                        await transaction.RollbackAsync();

                        // Hata mesajını daha anlamlı hale getirebiliriz
                        throw new InvalidOperationException("Ekip silinirken bir hata oluştu.", ex);
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
