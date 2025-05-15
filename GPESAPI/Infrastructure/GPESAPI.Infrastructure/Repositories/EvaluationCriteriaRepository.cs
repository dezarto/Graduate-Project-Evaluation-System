using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class EvaluationCriteriaRepository : IEvaluationCriteriaRepository
    {
        private readonly SqlDbContext _dbContext;

        public EvaluationCriteriaRepository(SqlDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(EvaluationCriteria evaluationCriteria)
        {
            if (evaluationCriteria == null)
                throw new ArgumentNullException(nameof(evaluationCriteria));

            await _dbContext.EvaluationCriterias.AddAsync(evaluationCriteria);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var evaluationCriteria = await _dbContext.EvaluationCriterias.FindAsync(id);
            if (evaluationCriteria == null)
                throw new ArgumentException($"EvaluationCriteria with ID {id} not found.");

            _dbContext.EvaluationCriterias.Remove(evaluationCriteria);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EvaluationCriteria>> GetAllAsync()
        {
            return await _dbContext.EvaluationCriterias.ToListAsync();
        }

        public async Task<EvaluationCriteria> GetByIdAsync(int id)
        {
            return await _dbContext.EvaluationCriterias
                .FirstOrDefaultAsync(ec => ec.CriteriaId == id);
        }

        public async Task UpdateAsync(EvaluationCriteria evaluationCriteria)
        {
            if (evaluationCriteria == null)
                throw new ArgumentNullException(nameof(evaluationCriteria));

            var existingEntity = await _dbContext.EvaluationCriterias
                .FindAsync(evaluationCriteria.CriteriaId);

            if (existingEntity == null)
                throw new ArgumentException($"EvaluationCriteria with ID {evaluationCriteria.CriteriaId} not found.");

            _dbContext.Entry(existingEntity).State = EntityState.Detached;

            _dbContext.EvaluationCriterias.Update(evaluationCriteria);
            await _dbContext.SaveChangesAsync();
        }
    }
}
