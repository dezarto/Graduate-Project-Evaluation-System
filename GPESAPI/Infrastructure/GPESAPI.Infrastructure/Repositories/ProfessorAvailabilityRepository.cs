using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;
using GraduateProjectEvaluationSystemAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GraduateProjectEvaluationSystemAPI.Infrastructure.Repositories
{
    public class ProfessorAvailabilityRepository : IProfessorAvailabilityRepository
    {
        private readonly SqlDbContext _dbContext;

        public ProfessorAvailabilityRepository(SqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckExistingAvailabilityAsync(int professorId, DateTime availableDate, TimeSpan startTime, TimeSpan endTime)
        {
            return await _dbContext.ProfessorAvailability
                .AnyAsync(a => a.ProfessorId == professorId
                    && a.AvailableDate == availableDate
                    && ((a.StartTime <= startTime && a.EndTime >= startTime) ||
                        (a.StartTime <= endTime && a.EndTime >= endTime)));
        }
    }
}
