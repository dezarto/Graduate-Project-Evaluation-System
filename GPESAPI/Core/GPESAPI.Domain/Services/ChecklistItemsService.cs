using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Domain.Services
{
    public class ChecklistItemsService : IChecklistItemsService
    {
        private readonly IGenericRepository<ChecklistItem> _reportRepository;
        public ChecklistItemsService(IGenericRepository<ChecklistItem> genericRepository)
        {
            _reportRepository = genericRepository;
        }

        public async Task AddChecklistItemsAsync(ChecklistItem checklistItems)
        {
            await _reportRepository.AddAsync(checklistItems);
            checklistItems.ItemId = checklistItems.ItemId;
        }

        public async Task DeleteChecklistItemsAsync(int id)
        {
            await _reportRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ChecklistItem>> GetAllChecklistItemsAsync()
        {
            return await _reportRepository.GetAllAsync();
        }

        public async Task<ChecklistItem> GetByChecklistItemsIdAsync(int id)
        {
            return await _reportRepository.GetByIdAsync(id);
        }

        public async Task UpdateChecklistItemsAsync(ChecklistItem checklistItems)
        {
            await _reportRepository.UpdateAsync(checklistItems);
        }
    }
}
