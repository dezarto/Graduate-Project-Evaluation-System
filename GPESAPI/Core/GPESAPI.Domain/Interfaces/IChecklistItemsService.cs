using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IChecklistItemsService
    {
        Task AddChecklistItemsAsync(ChecklistItem checklistItems);
        Task<IEnumerable<ChecklistItem>> GetAllChecklistItemsAsync();
        Task<ChecklistItem> GetByChecklistItemsIdAsync(int id);
        Task UpdateChecklistItemsAsync(ChecklistItem checklistItems);
        Task DeleteChecklistItemsAsync(int id);
    }
}
