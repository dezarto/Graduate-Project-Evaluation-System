using GPESAPI.Domain.Entities;

namespace GPESAPI.Domain.Interfaces
{
    public interface IChecklistItemDetailService
    {
        Task AddChecklistItemDetailAsync(ChecklistItemDetail checklistItemDetail);
        Task<IEnumerable<ChecklistItemDetail>> GetAllChecklistItemDetailAsync();
        Task<List<ChecklistItemDetail>> GetByChecklistItemDetailIdAsync(int id);
        Task UpdateChecklistItemDetailAsync(ChecklistItemDetail checklistItemDetail);
        Task DeleteChecklistItemDetailAsync(int id);
    }
}
