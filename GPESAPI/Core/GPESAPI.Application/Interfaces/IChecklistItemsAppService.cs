using GPESAPI.Application.DTOs;
using GPESAPI.Domain.Entities;

namespace GPESAPI.Application.Interfaces
{
    public interface IChecklistItemsAppService
    {
        Task AddChecklistItemsAsync(ChecklistItemDTO checklistItemsDto);
        Task<IEnumerable<ChecklistItemDTO>> GetAllChecklistItemsAsync();
        Task<ChecklistItemDTO> GetByChecklistItemsIdAsync(int id);
        Task UpdateChecklistItemsAsync(int id, ChecklistItemDTO checklistItemsDto);
        Task DeleteChecklistItemsAsync(int id);
    }
}
