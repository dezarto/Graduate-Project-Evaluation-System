using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class EvaluationCriteriaService : IEvaluationCriteriaService
    {
        private readonly IEvaluationCriteriaRepository _reportRepository;

        public EvaluationCriteriaService(IEvaluationCriteriaRepository genericRepository)
        {
            _reportRepository = genericRepository;
        }

        public async Task AddEvaluationCriteriaAsync(EvaluationCriteria evaluationCriteria)
        {
            await _reportRepository.AddAsync(evaluationCriteria);

            evaluationCriteria.CriteriaId = evaluationCriteria.CriteriaId;
        }

        public async Task DeleteEvaluationCriteriaAsync(int id)
        {
            await _reportRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<EvaluationCriteria>> GetAllEvaluationCriteriaAsync()
        {
            return await _reportRepository.GetAllAsync();
        }

        public async Task<EvaluationCriteria> GetByEvaluationCriteriaIdAsync(int id)
        {
            return await _reportRepository.GetByIdAsync(id);
        }

        public async Task UpdateEvaluationCriteriaAsync(EvaluationCriteria evaluationCriteria)
        {
            await _reportRepository.UpdateAsync(evaluationCriteria);
        }
    }
}
