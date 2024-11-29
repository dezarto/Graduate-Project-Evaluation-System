using AutoMapper;
using GPESAPI.Application.DTOs;
using GPESAPI.Application.Interfaces;
using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;

namespace GPESAPI.Application.Services
{
    public class ChecklistItemsAppService : IChecklistItemsAppService
    {
        private readonly IChecklistItemsService _checklistItemsService;
        private readonly IMapper _mapper;

        public ChecklistItemsAppService(IChecklistItemsService checklistItemsService, IMapper mapper)
        {
            _checklistItemsService = checklistItemsService;
            _mapper = mapper;
        }

        public async Task AddChecklistItemsAsync(ChecklistItemDTO checklistItem)
        {
            var checklistItemEntity = _mapper.Map<ChecklistItem>(checklistItem);

            await _checklistItemsService.AddChecklistItemsAsync(checklistItemEntity);

            checklistItem.ItemId = checklistItemEntity.ItemId;
        }

        public async Task DeleteChecklistItemsAsync(int id)
        {
            var existingItem = await _checklistItemsService.GetByChecklistItemsIdAsync(id);
            if (existingItem == null)
                throw new ArgumentException($"Checklist item with ID {id} not found.");

            await _checklistItemsService.DeleteChecklistItemsAsync(id);
        }

        public async Task<IEnumerable<ChecklistItemDTO>> GetAllChecklistItemsAsync()
        {
            var checklistItems = await _checklistItemsService.GetAllChecklistItemsAsync();

            return _mapper.Map<IEnumerable<ChecklistItemDTO>>(checklistItems);
        }

        public async Task<ChecklistItemDTO> GetByChecklistItemsIdAsync(int id)
        {
            var item = await _checklistItemsService.GetByChecklistItemsIdAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Checklist item with ID {id} not found.");

            return _mapper.Map<ChecklistItemDTO>(item);
        }

        public async Task UpdateChecklistItemsAsync(int id, ChecklistItemDTO checklistItem)
        {
            var existingItem = await _checklistItemsService.GetByChecklistItemsIdAsync(id);
            if (existingItem == null)
                throw new ArgumentException($"Checklist item with ID {id} not found.");

            var checklistItemEntity = _mapper.Map<ChecklistItem>(checklistItem);

            await _checklistItemsService.UpdateChecklistItemsAsync(checklistItemEntity);
        }
    }
}
