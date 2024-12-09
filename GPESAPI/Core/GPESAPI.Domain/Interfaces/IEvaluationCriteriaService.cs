using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IEvaluationCriteriaService
    {
        Task AddEvaluationCriteriaAsync(EvaluationCriteria evaluationCriteria);
        Task<IEnumerable<EvaluationCriteria>> GetAllEvaluationCriteriaAsync();
        Task<EvaluationCriteria> GetByEvaluationCriteriaIdAsync(int id);
        Task UpdateEvaluationCriteriaAsync(EvaluationCriteria evaluationCriteria);
        Task DeleteEvaluationCriteriaAsync(int id);
    }
}
