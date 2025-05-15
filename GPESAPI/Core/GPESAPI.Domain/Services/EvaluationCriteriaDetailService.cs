using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class EvaluationCriteriaDetailService : IEvaluationCriteriaDetailService
    {
        private readonly IGenericRepository<EvaluationCriteriaDetail> _repository;

        public EvaluationCriteriaDetailService(IGenericRepository<EvaluationCriteriaDetail> repository)
        {
            _repository = repository;
        }

        public async Task AddEvaluationCriteriaDetailAsync(EvaluationCriteriaDetail evaluationCriteriaDetail)
        {
            if (evaluationCriteriaDetail == null)
                throw new ArgumentNullException(nameof(evaluationCriteriaDetail), "EvaluationCriteriaDetail cannot be null.");

            await _repository.AddAsync(evaluationCriteriaDetail);
        }

        public async Task DeleteEvaluationCriteriaDetailAsync(int id)
        {
            var existingDetail = await _repository.GetByIdAsync(id);
            if (existingDetail == null)
                throw new KeyNotFoundException($"EvaluationCriteriaDetail with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<EvaluationCriteriaDetail>> GetAllEvaluationCriteriaDetailsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<EvaluationCriteriaDetail>> GetEvaluationCriteriaDetailByIdAsync(int id)
        {
            var detail = await _repository.GetByFieldAsync("EvaluationId", id);
            if (detail == null)
                throw new KeyNotFoundException($"EvaluationCriteriaDetail with ID {id} not found.");

            return detail;
        }

        public async Task UpdateEvaluationCriteriaDetailAsync(EvaluationCriteriaDetail evaluationCriteriaDetail)
        {
            if (evaluationCriteriaDetail == null)
                throw new ArgumentNullException(nameof(evaluationCriteriaDetail), "EvaluationCriteriaDetail cannot be null.");

            var existingDetail = await _repository.GetByIdAsync(evaluationCriteriaDetail.EvaluationId);
            if (existingDetail == null)
                throw new KeyNotFoundException($"EvaluationCriteriaDetail with ID {evaluationCriteriaDetail.EvaluationId} not found.");

            await _repository.UpdateAsync(evaluationCriteriaDetail);
        }
    }
}
