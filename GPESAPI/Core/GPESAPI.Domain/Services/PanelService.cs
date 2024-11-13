using GraduateProjectEvaluationSystemAPI.Domain.Entities;
using GraduateProjectEvaluationSystemAPI.Domain.Interfaces;

namespace GraduateProjectEvaluationSystemAPI.Domain.Services
{
    public class PanelService : IPanelService
    {
        private readonly IGenericRepository<Panel> _panelRepository;

        public PanelService(IGenericRepository<Panel> panelRepository)
        {
            _panelRepository = panelRepository;
        }

        public async Task<IEnumerable<Panel>> GetAllPanelsAsync()
        {
            return await _panelRepository.GetAllAsync();
        }

        public async Task<Panel> GetPanelByIdAsync(int id)
        {
            return await _panelRepository.GetByIdAsync(id);
        }

        public async Task AddPanelAsync(Panel panel)
        {
            await _panelRepository.AddAsync(panel);
        }

        public async Task UpdatePanelAsync(Panel panel)
        {
            await _panelRepository.UpdateAsync(panel);
        }

        public async Task DeletePanelAsync(int id)
        {
            await _panelRepository.DeleteAsync(id);
        }
    }

}
