using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IGenericRepository<Evaluation> _evaluationRepository;

        public EvaluationService(IGenericRepository<Evaluation> evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        public async Task<IEnumerable<Evaluation>> GetAllEvaluationAsync()
        {
            return await _evaluationRepository.GetAllAsync();
        }

        public async Task<Evaluation> GetEvaluationByIdAsync(int id)
        {
            return await _evaluationRepository.GetByIdAsync(id);
        }

        public async Task AddEvaluationAsync(Evaluation evaluation)
        {
            await _evaluationRepository.AddAsync(evaluation);
        }

        public async Task UpdateEvaluationAsync(Evaluation evaluation)
        {
            await _evaluationRepository.UpdateAsync(evaluation);
        }

        public async Task DeleteEvaluationAsync(int id)
        {
            await _evaluationRepository.DeleteAsync(id);
        }

        public async Task<List<Evaluation>> GetEvaluationByTeamIdAsync(int id)
        {
            return await _evaluationRepository.GetByFieldAsync("TeamId" , id);
        }

        public async Task<bool> HasMatchingProfessorAndTeamAsync(int professorId, int teamId)
        {
            var evaluations = await _evaluationRepository.GetAllAsync();
            return evaluations.Any(e => e.ProfessorId == professorId && e.TeamId == teamId);
        }
    }
}
