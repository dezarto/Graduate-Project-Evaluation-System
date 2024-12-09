using GPESAPI.Application.DTOs;

namespace GPESAPI.Application.Interfaces
{
    public interface IEvaluationAppService
    {
        Task<IEnumerable<EvaluationDTO>> GetAllEvaluationAppAsync();
        Task<EvaluationDTO> GetEvaluationAppByIdAsync(int id);
        Task AddEvaluationAppAsync(EvaluationDTO evaluationDto);
        Task UpdateEvaluationAppAsync(EvaluationDTO evaluationDto);
        Task DeleteEvaluationAppAsync(int id);
        Task<bool> SubmitEvaluationSave(EvaluateReasult evaluateResult, string professorMail);
        Task<EvaluateReasult> GetEvaluationResult(int evaluateId);
        Task<AllCriterias> GetAllCriterias();
    }
}
