using GraduateProjectEvaluationSystemAPI.Domain.Entities;

namespace GraduateProjectEvaluationSystemAPI.Domain.Interfaces
{
    public interface IEvaluationService
    {
        Task<IEnumerable<Evaluation>> GetAllEvaluationAsync();
        Task<Evaluation> GetEvaluationByIdAsync(int id);
        Task AddEvaluationAsync(Evaluation evaluation);
        Task UpdateEvaluationAsync(Evaluation evaluation);
        Task DeleteEvaluationAsync(int id);
    }
}
