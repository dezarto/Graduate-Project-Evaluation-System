using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IEvaluationService
    {
        Task<IEnumerable<Evaluation>> GetAllEvaluationAsync();
        Task<Evaluation> GetEvaluationByIdAsync(int id);
        Task<List<Evaluation>> GetEvaluationByTeamIdAsync(int id);
        Task AddEvaluationAsync(Evaluation evaluation);
        Task UpdateEvaluationAsync(Evaluation evaluation);
        Task DeleteEvaluationAsync(int id);
        Task<bool> HasMatchingProfessorAndTeamAsync(int professorId, int teamId);
    }
}
