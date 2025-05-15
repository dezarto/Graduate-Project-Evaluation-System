using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IEvaluationCriteriaRepository
    {
        Task<EvaluationCriteria> GetByIdAsync(int id);
        Task<IEnumerable<EvaluationCriteria>> GetAllAsync();
        Task AddAsync(EvaluationCriteria evaluationCriteria);
        Task UpdateAsync(EvaluationCriteria evaluationCriteria);
        Task DeleteAsync(int id);
    }
}
