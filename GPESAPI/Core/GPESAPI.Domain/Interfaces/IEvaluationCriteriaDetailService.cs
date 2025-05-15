using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IEvaluationCriteriaDetailService
    {
        Task<IEnumerable<EvaluationCriteriaDetail>> GetAllEvaluationCriteriaDetailsAsync();
        Task<List<EvaluationCriteriaDetail>> GetEvaluationCriteriaDetailByIdAsync(int id);
        Task AddEvaluationCriteriaDetailAsync(EvaluationCriteriaDetail evaluationCriteriaDetail);
        Task UpdateEvaluationCriteriaDetailAsync(EvaluationCriteriaDetail evaluationCriteriaDetail);
        Task DeleteEvaluationCriteriaDetailAsync(int id);
    }
}
