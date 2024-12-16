using GPESAPI.Domain.Entities;
using GPESAPI.Domain.Interfaces;
using GPESAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GPESAPI.Infrastructure.Repositories
{
    public class ChecklistItemsRepository : IChecklistItemsRepository
    {
        private readonly SqlDbContext _dbContext;

        public ChecklistItemsRepository(SqlDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(ChecklistItem checklistItem)
        {
            await _dbContext.ChecklistItems.AddAsync(checklistItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var checklistItem = await _dbContext.ChecklistItems.FindAsync(id);

            if (checklistItem != null)
            {
                _dbContext.ChecklistItems.Remove(checklistItem);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("ChecklistItem not found");
            }
        }

        public async Task<IEnumerable<ChecklistItem>> GetAllAsync()
        {
            return await _dbContext.ChecklistItems.ToListAsync();
        }

        public async Task<ChecklistItem> GetByIdAsync(int id)
        {
            return await _dbContext.ChecklistItems
                                    .FirstOrDefaultAsync(item => item.ItemId == id);
        }

        public async Task UpdateAsync(ChecklistItem checklistItem)
        {
            var existingItem = await _dbContext.ChecklistItems
                                                .FirstOrDefaultAsync(item => item.ItemId == checklistItem.ItemId);

            if (existingItem != null)
            {
                _dbContext.Entry(existingItem).CurrentValues.SetValues(checklistItem);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("ChecklistItem not found");
            }
        }
    }
}
