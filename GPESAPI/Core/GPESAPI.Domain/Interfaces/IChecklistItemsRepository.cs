using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IChecklistItemsRepository
    {
        Task<ChecklistItem> GetByIdAsync(int id);
        Task<IEnumerable<ChecklistItem>> GetAllAsync();
        Task AddAsync(ChecklistItem checklistItem);
        Task UpdateAsync(ChecklistItem checklistItem);
        Task DeleteAsync(int id);
    }
}
