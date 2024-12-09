using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class ChecklistItemDetailService : IChecklistItemDetailService
    {
        private readonly IGenericRepository<ChecklistItemDetail> _repository;

        public ChecklistItemDetailService(IGenericRepository<ChecklistItemDetail> repository)
        {
            _repository = repository;
        }

        public async Task AddChecklistItemDetailAsync(ChecklistItemDetail checklistItemDetail)
        {
            if (checklistItemDetail == null)
                throw new ArgumentNullException(nameof(checklistItemDetail), "ChecklistItemDetail cannot be null.");

            await _repository.AddAsync(checklistItemDetail);
        }

        public async Task DeleteChecklistItemDetailAsync(int id)
        {
            var existingDetail = await _repository.GetByIdAsync(id);
            if (existingDetail == null)
                throw new KeyNotFoundException($"ChecklistItemDetail with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ChecklistItemDetail>> GetAllChecklistItemDetailAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<ChecklistItemDetail>> GetByChecklistItemDetailIdAsync(int id)
        {
            var detail = await _repository.GetByFieldAsync("EvaluationId", id);
            if (detail == null)
                throw new KeyNotFoundException($"ChecklistItemDetail with ID {id} not found.");

            return detail;
        }

        public async Task UpdateChecklistItemDetailAsync(ChecklistItemDetail checklistItemDetail)
        {
            if (checklistItemDetail == null)
                throw new ArgumentNullException(nameof(checklistItemDetail), "ChecklistItemDetail cannot be null.");

            var existingDetail = await _repository.GetByIdAsync(checklistItemDetail.ItemId);
            if (existingDetail == null)
                throw new KeyNotFoundException($"ChecklistItemDetail with ID {checklistItemDetail.ItemId} not found.");

            await _repository.UpdateAsync(checklistItemDetail);
        }
    }
}
