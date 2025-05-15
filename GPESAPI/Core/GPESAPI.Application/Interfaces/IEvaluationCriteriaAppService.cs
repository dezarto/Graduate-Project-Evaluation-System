using GPESAPI.Application.DTOs;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Application.Interfaces
{
    public interface IEvaluationCriteriaAppService
    {
        Task AddEvaluationCriteriaAsync(EvaluationCriteriaDTO evaluationCriteriaDto);
        Task<IEnumerable<EvaluationCriteriaDTO>> GetAllEvaluationCriteriaAsync();
        Task<EvaluationCriteriaDTO> GetByEvaluationCriteriaIdAsync(int id);
        Task UpdateEvaluationCriteriaAsync(int id, EvaluationCriteriaDTO evaluationCriteriaDto);
        Task DeleteEvaluationCriteriaAsync(int id);
    }
}
