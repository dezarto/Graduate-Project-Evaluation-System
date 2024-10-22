using GraduateProjectEvaluationSystemAPI.Application.DTOs;

namespace GraduateProjectEvaluationSystemAPI.Application.Interfaces
{
    public interface IEvaluationAppService
    {
        Task<IEnumerable<EvaluationDTO>> GetAllEvaluationAppAsync();
        Task<EvaluationDTO> GetEvaluationAppByIdAsync(int id);
        Task AddEvaluationAppAsync(EvaluationDTO evaluationDto);
        Task UpdateEvaluationAppAsync(EvaluationDTO evaluationDto);
        Task DeleteEvaluationAppAsync(int id);
    }
}
